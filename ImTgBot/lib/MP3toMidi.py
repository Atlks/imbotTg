import aubio
import mido
from pydub import AudioSegment
from mido import MidiFile, MidiTrack, Message
#  pip install aubio mido pydub
# D:\0prj\mdsj\venv\Scripts\pip.exe  install aubio mido pydub
def convert_mp3_to_wav(mp3_path, wav_path):
    # 使用 pydub 将 MP3 转换为 WAV
    audio = AudioSegment.from_mp3(mp3_path)
    audio.export(wav_path, format="wav")

def audio_to_midi(audio_path, midi_path):
    # 设置 aubio 参数
    win_s = 1024                 # window size
    hop_s = win_s // 2           # hop size
    samplerate = 44100

    # 加载音频文件
    s = aubio.source(audio_path, samplerate, hop_s)
    samplerate = s.samplerate

    # 设置音高检测
    pitch_o = aubio.pitch("yin", win_s, hop_s, samplerate)
    pitch_o.set_unit("midi")
    pitch_o.set_silence(-40)

    # 创建一个新的 MIDI 文件
    midi = MidiFile()
    track = MidiTrack()
    midi.tracks.append(track)

    # 处理音频文件
    total_frames = 0
    while True:
        samples, read = s()
        pitch = pitch_o(samples)[0]
        total_frames += read

        if pitch:
            pitch_int = int(round(pitch))
            if 0 < pitch_int < 128:
                note_on = Message('note_on', note=pitch_int, velocity=64, time=0)
                track.append(note_on)
                note_off = Message('note_off', note=pitch_int, velocity=64, time=100)
                track.append(note_off)

        if read < hop_s:
            break

    # 保存 MIDI 文件
    midi.save(midi_path)

def convert_mp3_to_midi(mp3_path, midi_path):
    wav_path = "temp.wav"
    convert_mp3_to_wav(mp3_path, wav_path)
    audio_to_midi(wav_path, midi_path)

# 示例使用
convert_mp3_to_midi(r'D:\zzcdsk\wala man sayo ang lahat.mp3', r'wala man sayo ang lahat.mid')
