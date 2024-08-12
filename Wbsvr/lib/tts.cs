//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using System;
////using System.Speech.Synthesis;
//using Windows.Media.SpeechSynthesis;

//public static class TextToSpeech
//{
//    private static readonly SpeechSynthesizer Synthesizer = new SpeechSynthesizer();

//    static TextToSpeech()
//    {
//        // Set default properties if needed
//        Synthesizer.Volume = 100;  // 0...100
//        Synthesizer.Rate = 0;      // -10...10
//    }
//    }

//    public static void Speak(string text)
//    {
//        if (string.IsNullOrWhiteSpace(text))
//        {
//            throw new ArgumentException("Text cannot be null or whitespace.", nameof(text));
//        }

//        Synthesizer.
//    }

//    public static void SetVoice(string voiceName)
//    {
//        Synthesizer.SelectVoice(voiceName);
//    }

//    public static void SetRate(int rate)
//    {
//        Synthesizer.Rate = rate;
//    }

//    public static void SetVolume(int volume)
//    {
//        Synthesizer.Volume = volume;
//    }
//}
