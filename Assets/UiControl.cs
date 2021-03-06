﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Vuforia;
using System;

public class UiControl : MonoBehaviour {
    private Dropdown m_Dropdown;
    public Dictionary<string, List<GameObject>> myLegoDict = new Dictionary<string, List<GameObject>>();
	public float       scalingspeed = 0.01f;
	public bool        scaleUp      = false;
	public bool        scaleDown    = false;
	public int         step         = 1;
    public const int   lastStep     = 11;
    public const int   firstStep    = 1;
    public Dictionary<int, string[]> stepPieces = new Dictionary<int, string[]>()
    {
        {1, new string[] {"sprites/pin-base","sprites/base","None"} },
        {2, new string[] {"sprites/base-2","sprites/fogo-amarelo","sprites/fogo-laranja"} },
        {3, new string[] {"sprites/quadrangular-quatro-pinos","None", "None"}},
        {4, new string[] {"sprites/quadrangular-quatro-pinos","sprites/quadrangular-baixo-quatro-pinos","sprites/quadrangular-baixo-um-pino"}  },
        {5, new string[] {"sprites/triangulo-superior-lateral","sprites/retangular-pequeno-um-pino","None"} },
        {6, new string[] {"sprites/latera-azul","sprites/quadrangular-baixo-um-pino","sprites/triangulo-superior-lateral"} },
        {7, new string[] {"sprites/retangular-alto-pequeno-4-pinos","sprites/quadrangular-pequeno-um-pino","None"} },
        {8, new string[] {"sprites/ponta-foguete","sprites/retangular-alto-pequeno-1-pino","None"} },
        {9, new string[] {"sprites/retangular-dois-pinos","sprites/quadrangular-pequeno-um-pino","None"} },
        {10, new string[] {"sprites/triangular-arredondado-lateral-topo","None","None"} },
        {11, new string[] {"sprites/luz-azul","None","None"} },
    };
    // Use this for initialization
    void Start ()
    {
        //Fetch the Dropdown GameObject
        GameObject go = GameObject.FindGameObjectWithTag("step_selector");
        m_Dropdown = go.GetComponent<Dropdown>();
        //Add listener for when the value of the Dropdown changes, to take action
        m_Dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(m_Dropdown);
        });
        
        UpdateStepCounter();
        InitGameObjects();
        RenderStep();
    }

    // Update is called once per frame
    void Update () {

		if(scaleUp == true)
		{
			ScaleUpButton();
		}

        if (scaleDown == true)
        {
			ScaleDownButton();
		}
	}

	public void ScaleUpButton()
    {
		GameObject.FindWithTag("step_0").transform.localScale += new Vector3(scalingspeed,scalingspeed,scalingspeed);
	}

	public void ScaleDownButton()
    {
		GameObject.FindWithTag("step_0").transform.localScale -= new Vector3(scalingspeed,scalingspeed,scalingspeed);
	}

	public void PreviousStep()
	{
        if(step > firstStep)
        {
            step -= 1;
            UpdateStepCounter();
            UpdateStepPieces();
            // Select all GameObjects from next step and set active to false
            string tag = string.Format("step_{0}", step);
            print(tag);
            RenderStep();

            // Set view again
            if (step == 0)
            {
               // myLegoDict["step_0"][0].SetActive(true);
            }
        }
	}

	public void NextStep()
	{
        if (step < lastStep)
        {
            step += 1;
            UpdateStepCounter();
            UpdateStepPieces();

            // Disable full rocket view
            if (step > 0)
            {
                //myLegoDict["step_0"][0].SetActive(false);
            }

            // Select all GameObjects from current step and set active to true
            string tag = string.Format("step_{0}", step);
            print(tag);
            RenderStep(); 
        }
    }

    //Render step accordingly to new step
    private void DropdownValueChanged(Dropdown change)
    {
        // Set global step
        step = change.value + 1;
        // Render step
        RenderStep();
        // Update step counter
        UpdateStepCounter();
        UpdateStepPieces();

    }

    private void InitGameObjects()
    {
        for (int i = 0; i <= 11; i++)
        {
            tag = string.Format("step_{0}", i);
            print("Init: " + tag);
            foreach (GameObject go in GameObject.FindGameObjectsWithTag(tag).ToList())
            {
                if (!myLegoDict.ContainsKey(tag))
                {
                    myLegoDict.Add(tag, new List<GameObject>());
                }

                myLegoDict[tag].Add(go);

                go.SetActive(false);
            }
        }
    }

    private void RenderStep()
    {
        string tag = string.Empty;

        // Set all previous steps as visible
        for (int i = 1; i <= step; i++)
        {
            tag = string.Format("step_{0}", i);
            myLegoDict[tag].ForEach(go => go.SetActive(true));
        }

        // Set all next steps as inactive
        for (int i = step + 1; i <= lastStep; i++)
        {
            tag = string.Format("step_{0}", i);
            myLegoDict[tag].ForEach(go => go.SetActive(false));
        }
    }

    private void UpdateStepCounter()
	{
        m_Dropdown.value = step - 1;
        m_Dropdown.RefreshShownValue();
	}

    private void UpdateStepPieces()
    {

        GameObject slot_1;
        GameObject slot_2;
        GameObject slot_3;

        slot_1 = GameObject.FindGameObjectWithTag("slot_1");
        slot_2 = GameObject.FindGameObjectWithTag("slot_2");
        slot_3 = GameObject.FindGameObjectWithTag("slot_3");

        slot_1.GetComponent<SpriteRenderer>().sprite =  Resources.Load <Sprite> (stepPieces[step][0]);
        slot_2.GetComponent<SpriteRenderer>().sprite =  Resources.Load <Sprite> (stepPieces[step][1]);
        slot_3.GetComponent<SpriteRenderer>().sprite =  Resources.Load <Sprite> (stepPieces[step][2]);
    }
}
