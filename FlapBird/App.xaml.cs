namespace FlapBird;

public partial class App : Application
{
  public static Window CurrentWindow = null;
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

  protected override Window CreateWindow(IActivationState activationState)
  {
    return base.CreateWindow(activationState);
  }

}
