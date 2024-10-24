


using Plugin.Maui.Audio;

public class SoundHelper
{
  //------------------------------------------------------------------------

  public static void Play(string nomeArquivoWav)
  {
    Task.Run(async () =>
    {
      var morteFX = await FileSystem.OpenAppPackageFileAsync(nomeArquivoWav);
      var audioPlayer = AudioManager.Current.CreatePlayer(morteFX);
      audioPlayer.Play();
    });
  }

  //------------------------------------------------------------------------

}