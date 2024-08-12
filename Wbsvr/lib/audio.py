import librosa
import numpy as np
from midiutil import MIDIFile

def audio_to_midi(audio_path, midi_path):
    # Load the audio file
    y, sr = librosa.load(audio_path)
    
    # Use librosa's piptrack to estimate pitch
    pitches, magnitudes = librosa.piptrack(y=y, sr=sr)

    # Create a new MIDI file with one track
    midi = MIDIFile(1)
    track = 0
    time = 0
    midi.addTrackName(track, time, "Track")
    midi.addTempo(track, time, 120)

    # Iterate over time frames
    for t in range(pitches.shape[1]):
        # Get the pitch and magnitude for the current frame
        pitch = pitches[:, t]
        magnitude = magnitudes[:, t]
        
        # Only consider significant magnitudes
        if np.max(magnitude) > 0:
            index = magnitude.argmax()
            pitch_midi = int(librosa.hz_to_midi(pitch[index]))
            velocity = int(magnitude[index] * 127 / np.max(magnitudes))
            midi.addNote(track, 0, pitch_midi, t * 0.1, 1, velocity)

    # Write the MIDI file to disk
    with open(midi_path, "wb") as output_file:
        midi.writeFile(output_file)

# Example usage
audio_to_midi("input_audio.wav", "output_midi.mid")
