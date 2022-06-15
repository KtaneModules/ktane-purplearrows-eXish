using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using System;
using System.Text.RegularExpressions;

public class PurpleArrowsScript : MonoBehaviour
{

    public KMAudio audio;
    public KMBombInfo bomb;

    public KMColorblindMode Colorblind;
    public GameObject colorblindText;
    private bool colorblindMode;

    public KMSelectable[] buttons;
    public GameObject numDisplay;
    public GameObject wordDisplay;

    private string[] words = { "THESIS","IMMUNE","AGENCY","HEIGHT","ACTIVE","BOTHER","VIABLE","EXPOSE","BORDER",
                               "INSURE","INSIST","BEHAVE","THREAD","APATHY","OFFEND","EXTEND","VESSEL","EARWAX",
                               "OCCUPY","PRINCE","PARDON","WEIGHT","HARBOR","TRENCH","ABSORB","OUTFIT","INJURY",
                               "HONEST","REFUSE","ACCESS","PUNISH","VALLEY","WRITER","HAPPEN","BUCKET","AGENDA",
                               "BUBBLE","TYCOON","HEALTH","HAMMER","USEFUL","OFFSET","QUAINT","BOMBER","DETAIL",
                               "RESULT","ENERGY","PIGEON","EXCUSE","PLEASE","RELATE","APPEAR","THANKS","VISUAL",
                               "TRANCE","DINNER","THRONE","DANKER","WEALTH","JACKET","TUMBLE","WEAPON","WONDER",
                               "BOUNCE","HICCUP","UNIQUE","PRAYER","BRONZE","ENDURE","TIMBER","INSIDE","EMBARK",
                               "PLEDGE","POETRY","VELVET","WAITER","ESTATE","BELONG","IGNORE","HOTDOG","REGRET",
                               "ROTTEN","ADJUST","EXPAND","BORROW","TREATY","PLAYER","JUNIOR","WANDER","HELMET",
                               "IMPACT","BOTTOM","TICKET","GOSSIP","RETIRE","INFECT","DIRECT","BATTLE","DIVIDE",
                               "VIRTUE","UPDATE","PEANUT","IGNITE","QUEBEC","THRUST","ARTIST","ACCEPT","RANDOM",
                               "REMEDY","INSERT","HUNTER","TURKEY","WINNER","THEORY","IMPORT","OUTLET","BUFFET", };
    private int current;

    private string start;
    private string finish;
    private string finishscrambled;

    private bool cooldown = true;
    private bool isanimating = false;

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    void Awake()
    {
        current = 0;
        moduleId = moduleIdCounter++;
        moduleSolved = false;
        foreach (KMSelectable obj in buttons)
        {
            KMSelectable pressed = obj;
            pressed.OnInteract += delegate () { PressButton(pressed); return false; };
        }
    }

    void Start()
    {
        numDisplay.GetComponent<TextMesh>().text = " ";
        wordDisplay.GetComponent<TextMesh>().text = "------";
        GetComponent<KMBombModule>().OnActivate += OnActivate;
    }

    void OnActivate()
    {
        StartCoroutine(generateNewLet());
        colorblind();
    }

    void PressButton(KMSelectable pressed)
    {
        if (!moduleSolved && !cooldown)
        {
            pressed.AddInteractionPunch(0.25f);
            audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, pressed.transform);
            if (pressed == buttons[0])
            {
                if (currentIsUpSide())
                {
                    current += 108;
                }
                else
                {
                    current -= 9;
                }
                numDisplay.GetComponent<TextMesh>().text = "" + words[current].Substring(0, 1);
            }
            else if (pressed == buttons[1])
            {
                if (currentIsDownSide())
                {
                    current -= 108;
                }
                else
                {
                    current += 9;
                }
                numDisplay.GetComponent<TextMesh>().text = "" + words[current].Substring(0, 1);
            }
            else if (pressed == buttons[2])
            {
                if (currentIsLeftSide())
                {
                    current += 8;
                }
                else
                {
                    current -= 1;
                }
                numDisplay.GetComponent<TextMesh>().text = "" + words[current].Substring(0, 1);
            }
            else if (pressed == buttons[3])
            {
                if (currentIsRightSide())
                {
                    current -= 8;
                }
                else
                {
                    current += 1;
                }
                numDisplay.GetComponent<TextMesh>().text = "" + words[current].Substring(0, 1);
            }
            else if (pressed == buttons[4])
            {
                if (words[current].Equals(finish))
                {
                    moduleSolved = true;
                    StartCoroutine(victory());
                    Debug.LogFormat("[Purple Arrows #{0}] Pressed submit at '{1}'! That was correct!", moduleId, words[current]);
                }
                else
                {
                    GetComponent<KMBombModule>().HandleStrike();
                    Debug.LogFormat("[Purple Arrows #{0}] Pressed submit at '{1}'! That was incorrect!", moduleId, words[current]);
                    Debug.LogFormat("[Purple Arrows #{0}] Resetting Module...", moduleId);
                    numDisplay.GetComponent<TextMesh>().text = " ";
                    wordDisplay.GetComponent<TextMesh>().text = "------";
                    cooldown = true;
                    StartCoroutine(generateNewLet());
                }
            }
        }
    }

    private void colorblind()
    {
        colorblindMode = Colorblind.ColorblindModeActive;
        if (colorblindMode)
        {
            Debug.LogFormat("[Purple Arrows #{0}] Colorblind mode active!", moduleId);
            colorblindText.SetActive(true);
        }
    }

    private bool currentIsUpSide()
    {
        if ((current == 0) || (current == 1) || (current == 2) || (current == 3) || (current == 4) || (current == 5) || (current == 6) || (current == 7) || (current == 8))
        {
            return true;
        }
        return false;
    }

    private bool currentIsDownSide()
    {
        if ((current == 108) || (current == 109) || (current == 110) || (current == 111) || (current == 112) || (current == 113) || (current == 114) || (current == 115) || (current == 116))
        {
            return true;
        }
        return false;
    }

    private bool currentIsLeftSide()
    {
        if ((current == 0) || (current == 9) || (current == 18) || (current == 27) || (current == 36) || (current == 45) || (current == 54) || (current == 63) || (current == 72) || (current == 81) || (current == 90) || (current == 99) || (current == 108))
        {
            return true;
        }
        return false;
    }

    private bool currentIsRightSide()
    {
        if ((current == 8) || (current == 17) || (current == 26) || (current == 35) || (current == 44) || (current == 53) || (current == 62) || (current == 71) || (current == 80) || (current == 89) || (current == 98) || (current == 107) || (current == 116))
        {
            return true;
        }
        return false;
    }

    private IEnumerator generateNewLet()
    {
        yield return null;
        int rando = UnityEngine.Random.Range(0, 117);
        start = words[rando];
        current = rando;
        yield return new WaitForSeconds(0.5f);
        numDisplay.GetComponent<TextMesh>().text = "" + start.Substring(0, 1);
        int rando2 = rando;
        while (rando2 == rando)
        {
            rando2 = UnityEngine.Random.Range(0, 117);
            finish = words[rando2];
        }
        StopCoroutine("generateNewLet");
        StartCoroutine(scrambleFinish());
        Debug.LogFormat("[Purple Arrows #{0}] The starting word is '{1}'!", moduleId, start);
    }

    private IEnumerator scrambleFinish()
    {
        yield return null;
        char[] array = finish.ToCharArray();
        finishscrambled = finish;
        while (finishscrambled == finish)
        {
            System.Random rng = new System.Random();
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
            string scram = new string(array);
            finishscrambled = scram;
        }
        wordDisplay.GetComponent<TextMesh>().text = finishscrambled;
        cooldown = false;
        StopCoroutine("scrambleFinish");
        Debug.LogFormat("[Purple Arrows #{0}] The finishing word is '{1}'! It has been scrambled as '{2}'!", moduleId, finish, finishscrambled);
    }

    private IEnumerator victory()
    {
        yield return null;
        isanimating = true;
        wordDisplay.GetComponent<TextMesh>().text = "" + finish;
        for (int i = 0; i < 100; i++)
        {
            int rand1 = UnityEngine.Random.Range(0, 10);
            if (i < 50)
            {
                numDisplay.GetComponent<TextMesh>().text = rand1 + "";
            }
            else
            {
                numDisplay.GetComponent<TextMesh>().text = "G" + rand1;
            }
            yield return new WaitForSeconds(0.025f);
        }
        numDisplay.GetComponent<TextMesh>().text = "GG";
        StopCoroutine("victory");
        Debug.LogFormat("[Purple Arrows #{0}] Module Disarmed!", moduleId);
        GetComponent<KMBombModule>().HandlePass();
        isanimating = false;
    }

    //twitch plays
#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} u/d/l/r [Presses the specified arrow button] | !{0} submit [Submits the current word (position)] | Presses can be chained, for example '!{0} uuddlrl'";
#pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        if (Regex.IsMatch(command, @"^\s*submit\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            buttons[4].OnInteract();
            yield return new WaitForSeconds(.1f);
            if (moduleSolved) { yield return "solve"; }
            yield break;
        }

        string[] parameters = command.Split(' ');
        string checks = "";
        for (int j = 0; j < parameters.Length; j++)
        {
            checks += parameters[j];
        }
        var buttonsToPress = new List<KMSelectable>();
        for (int i = 0; i < checks.Length; i++)
        {
            if (checks.ElementAt(i).Equals('u') || checks.ElementAt(i).Equals('U'))
                buttonsToPress.Add(buttons[0]);
            else if (checks.ElementAt(i).Equals('d') || checks.ElementAt(i).Equals('D'))
                buttonsToPress.Add(buttons[1]);
            else if (checks.ElementAt(i).Equals('l') || checks.ElementAt(i).Equals('L'))
                buttonsToPress.Add(buttons[2]);
            else if (checks.ElementAt(i).Equals('r') || checks.ElementAt(i).Equals('R'))
                buttonsToPress.Add(buttons[3]);
            else
                yield break;
        }

        yield return null;
        foreach (KMSelectable km in buttonsToPress)
        {
            km.OnInteract();
            yield return new WaitForSeconds(.3f);
        }
    }

    private IEnumerator TwitchHandleForcedSolve()
    {
        while (cooldown)
            yield return true;
        var endPos = Array.IndexOf(words, finish);
        var x = Math.Abs((current % 9) - (endPos % 9)) >= 5 ? ((current % 9) - (endPos % 9) > 0 ? (current % 9) - (endPos % 9) - 9 : (current % 9) - (endPos % 9) + 9) : (current % 9) - (endPos % 9);
        var y = Math.Abs((current / 9) - (endPos / 9)) >= 7 ? ((current / 9) - (endPos / 9) > 0 ? (current / 9) - (endPos / 9) - 117 : (current / 9) - (endPos / 9) + 117) : (current / 9) - (endPos / 9);
        while (current % 9 != endPos % 9)
        {
            if (x < 0)
                buttons[3].OnInteract();
            if (x > 0)
                buttons[2].OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        while (current / 9 != endPos / 9)
        {
            if (y < 0)
                buttons[1].OnInteract();
            if (y > 0)
                buttons[0].OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        buttons[4].OnInteract();
        while (isanimating)
            yield return true;
    }
}
