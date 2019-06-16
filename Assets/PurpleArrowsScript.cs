using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using System;
using System.Text.RegularExpressions;

public class PurpleArrowsScript : MonoBehaviour {

    public KMAudio audio;
    public KMBombInfo bomb;

    public KMSelectable[] buttons;
    public GameObject numDisplay;
    public GameObject wordDisplay;

    private string[] words = { "THESIS","IMMUNE","AGENCY","HEIGHT","ACTIVE","BOTHER","VIABLE","EXPOSE","BORDER",
                               "INSURE","INSIST","BEHAVE","THREAD","APATHY","OFFEND","EXTEND","VESSEL","EARWAX",
                               "OCCUPY","PRINCE","PARDON","WEIGHT","HARBOR","TRENCH","ABSORB","OUTFIT","INJURY",
                               "HONEST","REFUSE","ACCESS","PUNISH","VALLEY","WRITER","HAPPEN","BUCKET","AGENDA",
                               "BUBBLE","TYCOON","HEALTH","HAMMER","USEFUL","OFFSET","QUAINT","BOMBER","DETAIL",
                               "RESULT","ENERGY","PIGEON","EXCUSE","PLEASE","RELATE","APPEAR","THANKS","VISUAL",
                               "TRANCE","DINNER","THRONE","ELAPSE","WEALTH","JACKET","TUMBLE","WEAPON","WONDER",
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

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    void Awake()
    {
        current = 0;
        moduleId = moduleIdCounter++;
        moduleSolved = false;
        foreach(KMSelectable obj in buttons){
            KMSelectable pressed = obj;
            pressed.OnInteract += delegate () { PressButton(pressed); return false; };
        }
    }

    void Start () {
        numDisplay.GetComponent<TextMesh>().text = " ";
        StartCoroutine(generateNewLet());
    }

    void PressButton(KMSelectable pressed)
    {
        if(moduleSolved != true)
        {
            pressed.AddInteractionPunch(0.25f);
            audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
            if (pressed == buttons[0])
            {
                if (currentIsUpSide())
                {
                    StartCoroutine(wallBump());
                }
                else
                {
                    current -= 9;
                    numDisplay.GetComponent<TextMesh>().text = "" + words[current].Substring(0, 1);
                }
            }
            else if (pressed == buttons[1])
            {
                if (currentIsDownSide())
                {
                    StartCoroutine(wallBump());
                }
                else
                {
                    current += 9;
                    numDisplay.GetComponent<TextMesh>().text = "" + words[current].Substring(0, 1);
                }
            }
            else if (pressed == buttons[2])
            {
                if (currentIsLeftSide())
                {
                    StartCoroutine(wallBump());
                }
                else
                {
                    current -= 1;
                    numDisplay.GetComponent<TextMesh>().text = "" + words[current].Substring(0, 1);
                }
            }
            else if (pressed == buttons[3])
            {
                if (currentIsRightSide())
                {
                    StartCoroutine(wallBump());
                }
                else
                {
                    current += 1;
                    numDisplay.GetComponent<TextMesh>().text = "" + words[current].Substring(0, 1);
                }
            }
            else if (pressed == buttons[4])
            {
                if (words[current].Equals(finish))
                {
                    StartCoroutine(victory());
                    Debug.LogFormat("[Purple Arrows #{0}] Pressed submit at '{1}'! That was correct!", moduleId, words[current]);
                }
                else
                {
                    GetComponent<KMBombModule>().HandleStrike();
                    Debug.LogFormat("[Purple Arrows #{0}] Pressed submit at '{1}'! That was incorrect!", moduleId, words[current]);
                    Debug.LogFormat("[Purple Arrows #{0}] Resetting Module...", moduleId);
                    Start();
                }
            }
        }
    }

    private bool currentIsUpSide()
    {
        if((current == 0) || (current == 1) || (current == 2) || (current == 3) || (current == 4) || (current == 5) || (current == 6) || (current == 7) || (current == 8))
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

    private IEnumerator wallBump()
    {
        yield return null;
        string store = words[current].Substring(0, 1);
        numDisplay.GetComponent<TextMesh>().text = " ";
        yield return new WaitForSeconds(0.25f);
        numDisplay.GetComponent<TextMesh>().text = ""+store;
        StopCoroutine("wallBump");
    }

    private IEnumerator generateNewLet()
    {
        yield return null;
        int rando = UnityEngine.Random.RandomRange(0, 117);
        start = words[rando];
        current = rando;
        yield return new WaitForSeconds(0.5f);
        numDisplay.GetComponent<TextMesh>().text = "" + start.Substring(0,1);
        int rando2 = rando;
        while(rando2 == rando)
        {
            rando2 = UnityEngine.Random.RandomRange(0, 117);
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
        wordDisplay.GetComponent<TextMesh>().text = finishscrambled;
        StopCoroutine("scrambleFinish");
        Debug.LogFormat("[Purple Arrows #{0}] The finishing word is '{1}'! It has been scrambled as '{2}'!", moduleId, finish, finishscrambled);
    }

    private IEnumerator victory()
    {
        yield return null;
        moduleSolved = true;
        wordDisplay.GetComponent<TextMesh>().text = "" + finish;
        for (int i = 0; i < 100; i++)
        {
            int rand1 = UnityEngine.Random.RandomRange(0, 10);
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
    }

    //twitch plays
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} up [Presses the up arrow button] | !{0} right [Presses the right arrow button] | !{0} down [Presses the down arrow button once] | !{0} left [Presses the left arrow button once] | !{0} left right down up [Chain button presses] | !{0} submit [Submits the current word (position)] | Direction words can be substituted as one letter (Ex. right as r)";
    #pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string command)
    {
        if (Regex.IsMatch(command, @"^\s*submit\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            yield return new[] { buttons[4] };
            yield break;
        }

        string[] parameters = command.Split(' ');
        var buttonsToPress = new List<KMSelectable>();
        foreach (string param in parameters)
        {
            if (param.EqualsIgnoreCase("up") || param.EqualsIgnoreCase("u"))
                buttonsToPress.Add(buttons[0]);
            else if (param.EqualsIgnoreCase("down") || param.EqualsIgnoreCase("d"))
                buttonsToPress.Add(buttons[1]);
            else if (param.EqualsIgnoreCase("left") || param.EqualsIgnoreCase("l"))
                buttonsToPress.Add(buttons[2]);
            else if (param.EqualsIgnoreCase("right") || param.EqualsIgnoreCase("r"))
                buttonsToPress.Add(buttons[3]);
            else
                yield break;
        }

        yield return null;
        foreach(KMSelectable bm in buttonsToPress)
        {
            bm.OnInteract();
            yield return new WaitForSeconds(0.5f);
        }
    }
}
