using UnityEngine;

public class MicPitchClassifier : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField] string deviceName = "";
    [SerializeField] int sampleRate = 44100;
    [SerializeField] int fftSize = 2048;
    [SerializeField] float minHz = 80f;
    [SerializeField] float maxHz = 500f;
    [SerializeField] float mediumLow = 140f;
    [SerializeField] float mediumHigh = 300f;
    [SerializeField] float hyst = 10f;
    [SerializeField] float minThreshold = 0.0005f;
    [SerializeField] float thresholdMul = 4f;
    [SerializeField] float calibrateSeconds = 1f;

    string mic;
    float[] spectrum;
    float energyEMA;
    float noiseEMA;
    float lastFreq;
    string lastCat = "";
    float calibrateTimer;
    bool calibrated;

    void OnValidate()
    {
        fftSize = Mathf.Clamp(Mathf.ClosestPowerOfTwo(Mathf.Max(fftSize, 64)), 64, 8192);
    }

    void Start()
    {
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        if (Microphone.devices.Length == 0) { Debug.LogError("No mic"); enabled = false; return; }
        mic = string.IsNullOrEmpty(deviceName) ? Microphone.devices[0] : deviceName;
        spectrum = new float[fftSize];
        audioSource.clip = Microphone.Start(mic, true, 10, sampleRate);
        audioSource.loop = true;
        audioSource.mute = true;
        StartCoroutine(W());
    }

    System.Collections.IEnumerator W()
    {
        while (Microphone.GetPosition(mic) <= 0) yield return null;
        audioSource.Play();
    }

    void Update()
    {
        if (audioSource.clip == null) return;
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        int minBin = Mathf.Max(1, Mathf.FloorToInt(minHz * fftSize / sampleRate));
        int maxBin = Mathf.Min(fftSize / 2 - 2, Mathf.CeilToInt(maxHz * fftSize / sampleRate));

        float bandEnergy = 0f;
        for (int i = minBin; i <= maxBin; i++) bandEnergy += spectrum[i];
        energyEMA = Mathf.Lerp(energyEMA, bandEnergy, 0.2f);

        if (!calibrated)
        {
            calibrateTimer += Time.deltaTime;
            noiseEMA = Mathf.Lerp(noiseEMA, bandEnergy, 0.2f);
            if (calibrateTimer >= calibrateSeconds) calibrated = true;
            return;
        }

        float gate = Mathf.Max(minThreshold, noiseEMA * thresholdMul);
        noiseEMA = Mathf.Lerp(noiseEMA, bandEnergy, 0.02f);

        if (energyEMA < gate)
        {
            if (!string.IsNullOrEmpty(lastCat)) { lastCat = ""; Debug.Log("Silence"); }
            return;
        }

        int iMax = minBin; float vMax = 0f;
        for (int i = minBin; i <= maxBin; i++) if (spectrum[i] > vMax) { vMax = spectrum[i]; iMax = i; }
        int iL = Mathf.Max(iMax - 1, minBin);
        int iR = Mathf.Min(iMax + 1, maxBin);
        float yL = spectrum[iL], yC = spectrum[iMax], yR = spectrum[iR];
        float p = 0f; float d = (yL - 2f * yC + yR);
        if (Mathf.Abs(d) > 1e-9f) p = 0.5f * (yL - yR) / d;
        float bin = Mathf.Clamp(iMax + p, minBin, maxBin);
        float freq = bin * sampleRate / fftSize;
        if (freq < minHz || freq > maxHz) return;

        float lowUp = mediumLow + hyst, lowDown = mediumLow - hyst;
        float highUp = mediumHigh + hyst, highDown = mediumHigh - hyst;

        string cat = lastCat;
        if (string.IsNullOrEmpty(cat))
            cat = freq < mediumLow ? "LowPitch" : (freq <= mediumHigh ? "MediumPitch" : "HighPitch");
        else if (cat == "LowPitch" && freq > lowUp) cat = "MediumPitch";
        else if (cat == "MediumPitch" && freq < lowDown) cat = "LowPitch";
        else if (cat == "MediumPitch" && freq > highUp) cat = "HighPitch";
        else if (cat == "HighPitch" && freq < highDown) cat = "MediumPitch";

        if (cat != lastCat || Mathf.Abs(freq - lastFreq) > 30f)
        {
            Debug.Log($"{cat} ({freq:F0} Hz)");
            lastCat = cat;
            lastFreq = freq;
        }
    }
}
