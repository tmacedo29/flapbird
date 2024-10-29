


using Plugin.Maui.Audio;

public class SoundHelper
{
  //------------------------------------------------------------------------

  public static void Play(string nomeArquivoWav, bool loop = false)
  {
    Task.Run(async () =>
    {
      var audioFX = await FileSystem.OpenAppPackageFileAsync(nomeArquivoWav);
      var audioPlayer = AudioManager.Current.CreatePlayer(audioFX);
      audioPlayer.Loop = loop;
      audioPlayer.Play();
    });
  }

  //------------------------------------------------------------------------

}