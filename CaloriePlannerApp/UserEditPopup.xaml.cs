using CaloriePlannerApp.Data;
using CaloriePlannerApp.Data.Entities;
using CaloriePlannerApp.Data.Model.Enums;
using CaloriePlannerApp.Data.Services;
using CaloriePlannerApp.Data.ViewModels;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CaloriePlannerApp;

public partial class UserEditVm : ObservableObject
{
    private readonly DataContext context;
    private Guid? id;
    private bool isValid;
    Action close;
    Action<UserModel> saved;
    public UserEditVm(DataContext dataContext, Guid? id, Action close, Action<UserModel> saved)
    {
        context = dataContext;
        this.id = id;
        this.close = close;
        this.saved = saved;
    }
    [ObservableProperty]
    private DateTime birth = new DateTime();
    [ObservableProperty]
    private string firstName;
    [ObservableProperty]
    private string lastName;
    [ObservableProperty]
    private decimal height;
    [ObservableProperty]
    private decimal weight;
    [ObservableProperty]
    private string activityLevelString = ActivityLevelEnum.HIGH.ToString();
    [ObservableProperty]
    private string progressTypeString = ProgressTypeEnum.LOW_RATE.ToString();
    [ObservableProperty]
    private string weightGoalString = WeightGoalEnum.MAINTAIN.ToString();
    [ObservableProperty]
    private int calorieGoal;
    [ObservableProperty]
    private List<string> activityLevels;

    [ObservableProperty]
    private List<string> progressTypes;

    [ObservableProperty]
    private List<string> weightGoals;
    [ObservableProperty]
    private string message;

    public async Task Init()
    {
        ActivityLevels = Enum.GetValues(typeof(ActivityLevelEnum)).Cast<ActivityLevelEnum>().ToList().Select(x => x.ToString()).ToList();
        ProgressTypes = Enum.GetValues(typeof(ProgressTypeEnum)).Cast<ProgressTypeEnum>().ToList().Select(x => x.ToString()).ToList();
        WeightGoals = Enum.GetValues(typeof(WeightGoalEnum)).Cast<WeightGoalEnum>().ToList().Select(x => x.ToString()).ToList();

        if (id.HasValue)
        {
            var user = context.Users.FirstOrDefault(x => x.Id == id.Value);
            if (user != null)
            {
                Birth = user.Birth;
                FirstName = user.FirstName;
                LastName = user.LastName;
                Height = user.Height;
                Weight = user.Weight;
                ActivityLevelString = user.ActivityLevelEnum.ToString();
                ProgressTypeString = user.ProgressTypeEnum.ToString();
                WeightGoalString = user.WeightGoalEnum.ToString();
                CalorieGoal = user.CalorieGoal;
            }
        }
        this.PropertyChanged += (a, b) =>
        {
            if (b.PropertyName != "Message")
            {
                EvaluateForm();
            }
        };
        EvaluateForm();
    }

    public void EvaluateForm()
    {
        Message = "";

        bool isValid = true;

        if (Height <= 0)
        {
            Message += "Height must be greater than 0. ";
            isValid = false;
        }
        else if (Weight <= 0)
        {
            Message += "Weight must be greater than 0. ";
            isValid = false;
        }
        else if (Birth == null)
        {
            Message += "Birthdate is required. ";
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(FirstName))
        {
            Message += "First name is required. ";
            isValid = false;
        }
        if (string.IsNullOrWhiteSpace(LastName))
        {
            Message += "Last name is required. ";
            isValid = false;
        }

        if (isValid)
        {
            this.isValid = isValid;
        }
        CalculateDailyCalorieGoal();
    }

    public void CalculateDailyCalorieGoal()
    {
        var age = DateTime.Now.Year - Birth.Year;
        if (DateTime.Now.DayOfYear < Birth.DayOfYear)
            age--;

        // Calculate BMR (Basal Metabolic Rate) using Mifflin-St Jeor formula
        decimal bmr = 10 * Weight + 6.25m * Height - 5 * age + 161;

        // Parse ActivityLevelEnum (string to enum)
        if (Enum.TryParse(ActivityLevelString, out ActivityLevelEnum aclevel))
        {
            decimal activityMultiplier = aclevel switch
            {
                ActivityLevelEnum.LIGHT => 1.375m,
                ActivityLevelEnum.MODERATE => 1.55m,
                ActivityLevelEnum.HIGH => 1.725m,
                _ => 1.2m, // Default to Sedentary if unknown
            };

            // Apply the activity multiplier to BMR
            decimal adjustedBMR = bmr * activityMultiplier;

            // Parse WeightGoalEnum (string to enum)
            if (Enum.TryParse(WeightGoalString, out WeightGoalEnum weightGoal))
            {
                adjustedBMR += weightGoal switch
                {
                    WeightGoalEnum.LOSE => -500,  // Calorie deficit for weight loss
                    WeightGoalEnum.GAIN => 500,   // Calorie surplus for weight gain
                    _ => 0,   // No change for MAINTAIN
                };
            }

            // Parse ProgressTypeEnum (string to enum)
            if (Enum.TryParse(ProgressTypeString, out ProgressTypeEnum progressType))
            {
                adjustedBMR += progressType switch
                {
                    ProgressTypeEnum.HIGH_RATE => 200,  // High rate - increase by 200 calories
                    ProgressTypeEnum.MODERATE_RATE => 100,  // Moderate rate - increase by 100 calories
                    ProgressTypeEnum.LOW_RATE => -100,   // Low rate - decrease by 100 calories
                    _ => 0,   // No change for other progress types
                };
            }

            // Set the final calorie goal
            CalorieGoal = (int)Math.Round(adjustedBMR);
        }
        else
        {
            // If parsing fails, set an appropriate message or handle accordingly
            Message = "Invalid Activity Level Enum value.";
        }
    }


    [RelayCommand]
    public void Save()
    {
        if (isValid)
        {
            if (id.HasValue)
            {
                var user = context.Users.FirstOrDefault(x => x.Id == id.Value);
                if (user != null)
                {
                    user.FirstName = FirstName;
                    user.LastName = LastName;
                    user.Birth = Birth;
                    user.ActivityLevelEnum = Enum.Parse<ActivityLevelEnum>(ActivityLevelString);
                    user.ProgressTypeEnum = Enum.Parse<ProgressTypeEnum>(ProgressTypeString); 
                    user.WeightGoalEnum = Enum.Parse<WeightGoalEnum>(WeightGoalString); 
                    user.Weight = Weight;
                    user.Height = Height;
                    user.CalorieGoal = CalorieGoal;
                    context.SaveChanges();
                    this.saved.Invoke(new UserModel()
                    {
                        Id = user.Id,
                        Name = user.UserFullName()
                    });
                    this.close.Invoke();
                }

            }
            else
            {
                var usernew = new User()
                {
                    ActivityLevelEnum = Enum.Parse<ActivityLevelEnum>(ActivityLevelString),
                    Birth = Birth,
                    CalorieGoal = CalorieGoal,
                    FirstName = FirstName,
                    Height = Height,
                    Id = Guid.NewGuid(),
                    LastName = LastName,
                    ProgressTypeEnum = Enum.Parse<ProgressTypeEnum>(ProgressTypeString),
                    Weight = Weight,
                    WeightGoalEnum = Enum.Parse<WeightGoalEnum>(WeightGoalString),
                };

                context.Users.Add(usernew);
                context.SaveChanges();
                this.saved.Invoke(new UserModel()
                {
                    Id = usernew.Id,
                    Name = usernew.UserFullName()
                });
                this.close.Invoke();
            }
        }
    }
    [RelayCommand]
    public void Close()
    {
        this.close.Invoke();
    }
}

public partial class UserEditPopup : Popup
{
    private UserEditVm vm;
    public UserEditPopup(DataContext dataContext, Guid? id, Action<UserModel> saved)
    {
        vm = new UserEditVm(dataContext, id, () => this.Close(), saved);
        BindingContext = vm;
        InitializeComponent();
    }
    protected override async void OnParentSet()
    {
        await vm.Init();
        base.OnParentSet();
    }
}