using AgeOfHeroes.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class CharacterA : Pawn
    {
        // Start is called before the first frame update

        // Update is called once per frame
        void Update()
        {


        }

        public override void Attack(int attackType)
        {
            base.Attack(attackType);
            switch (attackType)
            {
                case 0:
                    StartCoroutine(Co_CloseAttack_1());
                    break;
                case 1:
                    StartCoroutine(Co_CloseAttack_1());
                    break;
                case 2:
                    StartCoroutine(Co_CloseAttack_1());
                    break;
                case 3:
                    break;

            }
        }



        public override void HandleAnimEvent(string eventName)
        {
            base.HandleAnimEvent(eventName);

            switch (eventName)
            {
                case "hit1":
                    HitEvent_1();
                    break;
                case "hit2":
                    HitEvent_1();
                    break;
                case "hit3":
                    HitEvent_1();
                    break;
            }
        }
    }
}