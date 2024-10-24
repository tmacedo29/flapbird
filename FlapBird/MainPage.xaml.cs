using Plugin.Maui.Audio;

namespace FlapBird;

public partial class MainPage : ContentPage
{
  int velocidade = 10; // 10px/frame 
  const int gravidade = 20; // 30px/frame
  const int forcaPulo = 40; // 60px/frame
  const int maxTempoPulando = 3; // 3 frame

  const int tamanhoMinimoPassagem = 200;


  int score = 0;
  int tempoEntreFrames = 50;
  double larguraJanela =  0;
  double alturaJanela =  0;
  bool estaMorto = true;
  bool estaPulando = false;
  int tempoPulando = 0;

  //---------------------------------------------------------------------------------------------------------------

	public MainPage()
	{
		InitializeComponent();
	}

  //---------------------------------------------------------------------------------------------------------------

  protected override void OnAppearing()
  {
    base.OnAppearing();
  }

  //---------------------------------------------------------------------------------------------------------------

  protected override void OnSizeAllocated(double width, double height)
  {
    base.OnSizeAllocated(width, height);

    larguraJanela = width;
    alturaJanela = height;
    if (height > 0)
    {
      imgCanoCima.HeightRequest  = height - imgChao.HeightRequest;
      imgCanoBaixo.HeightRequest = height - imgChao.HeightRequest;
    }
  }

  //---------------------------------------------------------------------------------------------------------------

  void Inicializar()
  {
    imgCanoBaixo.TranslationX = -larguraJanela;
    imgCanoCima.TranslationX  = -larguraJanela;
    imgPardal.TranslationX    = 0;
    imgPardal.TranslationY    = 0;
    score = 0;

    GerenciaCanos();
  }

  //---------------------------------------------------------------------------------------------------------------

  private async Task Desenhar()
  {
    while(!estaMorto)
    {
      if (estaPulando)
        AplicaPulo();
      else
        AplicaGravidade();

      GerenciaCanos();

      if (VerificaColisao())
      {
        estaMorto = true;
        labelGameOverScore.Text = "Você passou por\n" + score + " canos";
        frameOverlay.IsVisible = true;
        break;
      }

      await Task.Delay(tempoEntreFrames);
    }
  }

  //---------------------------------------------------------------------------------------------------------------

  void GerenciaCanos()
  {
    imgCanoCima.TranslationX -= velocidade;
    imgCanoBaixo.TranslationX -= velocidade;
    if (imgCanoBaixo.TranslationX < -larguraJanela)
    {
      imgCanoBaixo.TranslationX = 0;
      imgCanoCima.TranslationX = 0;

      var alturaMaxima = -(imgCanoBaixo.HeightRequest * 0.1);
      var alturaMinima = -(imgCanoBaixo.HeightRequest * 0.8);

      imgCanoCima.TranslationY  = Random.Shared.Next((int)alturaMinima, (int)alturaMaxima);
      imgCanoBaixo.TranslationY = imgCanoCima.HeightRequest + imgCanoCima.TranslationY + tamanhoMinimoPassagem;

      score++;
      labelScore.Text = "Score: " + score.ToString("D5");
      if (score % 4 == 0)
        velocidade++;
    }
  }

  //---------------------------------------------------------------------------------------------------------------

  void AplicaGravidade()
  {
    imgPardal.TranslationY += gravidade;
  }

  //---------------------------------------------------------------------------------------------------------------

  void AplicaPulo()
  {
    imgPardal.TranslationY -= forcaPulo;
    tempoPulando++;
    if (tempoPulando >= maxTempoPulando)
    {
      tempoPulando = 0;
      estaPulando = false;
    }
  }

  //---------------------------------------------------------------------------------------------------------------

  bool VerificaColisao()
  {
    return (!estaMorto && (VerificaColisaoChao() || VerificaColisaoTeto() || VerificaColisaoCano()));
  }

  //---------------------------------------------------------------------------------------------------------------

  bool VerificaColisaoCano()
  {
    if (VerificaColisaoCanoBaixo() || VerificaColisaoCanoCima())
      return true;
    else
      return false;
  }

  //---------------------------------------------------------------------------------------------------------------

  bool VerificaColisaoCanoCima()
  {
    var posicaoHorizontalPardal = (larguraJanela - 50) - (imgPardal.WidthRequest / 2);
    var posicaoVerticalPardal   = (alturaJanela / 2) - (imgPardal.HeightRequest / 2) + imgPardal.TranslationY;

    if (
         posicaoHorizontalPardal >= Math.Abs(imgCanoCima.TranslationX) - imgCanoCima.WidthRequest &&
         posicaoHorizontalPardal <= Math.Abs(imgCanoCima.TranslationX) + imgCanoCima.WidthRequest &&
         posicaoVerticalPardal   <= imgCanoCima.HeightRequest + imgCanoCima.TranslationY
       )
      return true;
    else
      return false;
  }

  //---------------------------------------------------------------------------------------------------------------

  bool VerificaColisaoCanoBaixo()
  {
    var posicaoHorizontalPardal = larguraJanela - 50 - imgPardal.WidthRequest / 2;
    var posicaoVerticalPardal   = (alturaJanela / 2) + (imgPardal.HeightRequest / 2) + imgPardal.TranslationY;

    var yMaxCano = imgCanoCima.HeightRequest + imgCanoCima.TranslationY + tamanhoMinimoPassagem;

    if (
         posicaoHorizontalPardal >= Math.Abs(imgCanoCima.TranslationX) - imgCanoCima.WidthRequest &&
         posicaoHorizontalPardal <= Math.Abs(imgCanoCima.TranslationX) + imgCanoCima.WidthRequest &&
         posicaoVerticalPardal   >= yMaxCano
       )
      return true;
    else
      return false;
  }

  //---------------------------------------------------------------------------------------------------------------

  bool VerificaColisaoChao()
  {
    var alturaDoTeto = alturaJanela / 2;
    if (imgPardal.TranslationY >= alturaDoTeto - imgChao.Height)
      return true;
    else
      return false;
  }

  //---------------------------------------------------------------------------------------------------------------

  bool VerificaColisaoTeto()
  {
    var alturaDoTeto = alturaJanela / 2;
    if (imgPardal.TranslationY <= -alturaDoTeto)
      return true;
    else
      return false;
  }

  //---------------------------------------------------------------------------------------------------------------

  private void OnFrameClicked(object sender, TappedEventArgs e)
  {
    frameOverlay.IsVisible = false;
    Inicializar();
    estaMorto = false;
    Desenhar().RunSynchronously();
  }

  //---------------------------------------------------------------------------------------------------------------

  private void OnGridClicked(object sender, TappedEventArgs e)
  {
    estaPulando = true;
  }

  //---------------------------------------------------------------------------------------------------------------
}

