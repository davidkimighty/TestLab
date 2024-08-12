using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct RandomWeapon
{
    public WeaponFactory Weapon;
    public float ScoreCut;
}

public class ZombieFightManager : MonoBehaviour
{
    [SerializeField] private RandomWeapon[] randomWeapons;
    [SerializeField] private Human human;
    [SerializeField] private Zombie zom;

    [SerializeField] private InputActionReference leftInput;
    [SerializeField] private InputActionReference rightInput;
    [SerializeField] private InputActionReference downInput;
    [SerializeField] private InputActionReference upInput;
    [SerializeField] private int keyMaxSpawn = 10;
    [SerializeField] private Key keyPrefab;
    [SerializeField] private Transform keyHolder;

    [SerializeField] private TMP_Text scoreText;
    
    private float singleKeyScore;
    private float currentScore;
    private Queue<Key> activeKeys = new();
    private IWeapon weapon;

    private IEnumerator Start()
    {
        singleKeyScore = 100f / keyMaxSpawn;
        scoreText.text = currentScore.ToString();

        yield return SceneIntro();
        yield return SummonWeapon();
    }

    private IEnumerator SceneIntro()
    {
        for (int i = 0; i < keyMaxSpawn; i++)
        {
            Key key = Instantiate(keyPrefab, keyHolder);
            key.RandomKey();
            activeKeys.Enqueue(key);
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    private void KeyPressed(Keys key)
    {
        if (activeKeys.Count == 0) return;
        
        Key currentKey = activeKeys.Peek();
        if (currentKey.KeyValue != key)
        {
            currentScore = Mathf.Clamp(currentScore - singleKeyScore, 0, 100f);
            currentKey.WrongPress();
        }
        else
        {
            currentScore = Mathf.Clamp(currentScore + singleKeyScore, 0, 100f);
            currentKey.CorrectPress();
            activeKeys.Dequeue();
        }
        scoreText.text = currentScore.ToString();
    }

    private IEnumerator SummonWeapon()
    {
        Action<InputAction.CallbackContext> leftAction = (c) => KeyPressed(Keys.Left);
        Action<InputAction.CallbackContext> rightAction = (c) => KeyPressed(Keys.Right);
        Action<InputAction.CallbackContext> downAction = (c) => KeyPressed(Keys.Down);
        Action<InputAction.CallbackContext> upAction = (c) => KeyPressed(Keys.Up);
        
        leftInput.action.performed += leftAction;
        rightInput.action.performed += rightAction;
        downInput.action.performed += downAction;
        upInput.action.performed += upAction;
        
        while (activeKeys.Count != 0)
        {
            yield return null;
        }
        weapon = CreateWeapon();
        human.EquipWeapon(weapon);
        
        leftInput.action.performed -= leftAction;
        rightInput.action.performed -= rightAction;
        downInput.action.performed -= downAction;
        upInput.action.performed -= upAction;
    }
    
    private IWeapon CreateWeapon()
    {
        float cumulativeScore = 0;
        for (int i = 0; i < randomWeapons.Length; i++)
        {
            RandomWeapon randomWeapon = randomWeapons[i];
            cumulativeScore += randomWeapon.ScoreCut;
            
            if (currentScore <= cumulativeScore)
                return randomWeapon.Weapon.Create();
        }
        return null;
    }
}
