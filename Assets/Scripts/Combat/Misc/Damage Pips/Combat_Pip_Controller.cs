using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Pip_Controller : MonoBehaviour
{
    private List<GameObject> playerAttackingPips = new List<GameObject>();
    private List<GameObject> playerDefendingPips = new List<GameObject>();

    private List<GameObject> enemyAttackingPips = new List<GameObject>();
    private List<GameObject> enemyDefendingPips = new List<GameObject>();

    [SerializeField]
    private GameObject pip;

    public IEnumerator ReleaseThePips(Field_Full_Results results, int pos)
    {
        Field_Card_Results playerRes = results.GetPlayerResult(pos);
        Field_Card_Results enemyRes = results.GetEnemyResult(pos);

        int playerAttPips = PipAmount(playerRes.finalPower);
        int enemyAttPips = PipAmount(enemyRes.finalPower);

        float playerAttThroughRate = Mathf.Min(playerRes.totalDamage / playerRes.finalPower, 1);
        float enemyAttThroughRate = Mathf.Min(enemyRes.totalDamage / enemyRes.finalPower, 1);

        int playerBlockPips = Mathf.FloorToInt(enemyAttPips * (1 - enemyAttThroughRate));
        int enemyBlockPips = Mathf.FloorToInt(playerAttPips * (1 - playerAttThroughRate));

        for (int i = 0; i < playerAttPips; i++)
        {
            GameObject tempPip = Instantiate(this.pip, playerRes.card.transform.position, Quaternion.identity);

            Transform enemyPos = GameManager.instance.CUI.uiCoordinator.GetEnemySpriteObject().transform;
            if (i < enemyBlockPips)
            {
                GameObject tempBlockPip = Instantiate(this.pip, enemyRes.card.transform.position, Quaternion.identity);

                tempPip.GetComponent<Damage_Pip_Movement>().SetupDefender(tempBlockPip.transform, enemyPos);
                tempBlockPip.GetComponent<Damage_Pip_Movement>().SetupDefender(tempPip.transform, tempPip.transform);

                this.playerDefendingPips.Add(tempPip);
                this.enemyDefendingPips.Add(tempBlockPip);
            } 
            else
            {
                tempPip.GetComponent<Damage_Pip_Movement>().SetupAttacker(enemyPos, enemyPos);

                this.playerAttackingPips.Add(tempPip);
            }
        }

        for (int i = 0; i < enemyAttPips; i++)
        {
            GameObject tempPip = Instantiate(this.pip, enemyRes.card.transform.position, Quaternion.identity);

            Transform playerPos = GameManager.instance.CUI.uiCoordinator.GetPlayerSpriteObject().transform;
            if (i < playerBlockPips)
            {
                GameObject tempBlockPip = Instantiate(this.pip, playerRes.card.transform.position, Quaternion.identity);

                tempPip.GetComponent<Damage_Pip_Movement>().SetupDefender(tempBlockPip.transform, playerPos);
                tempBlockPip.GetComponent<Damage_Pip_Movement>().SetupDefender(tempPip.transform, tempPip.transform);

                this.enemyDefendingPips.Add(tempPip);
                this.playerDefendingPips.Add(tempBlockPip);
            }
            else
            {
                tempPip.GetComponent<Damage_Pip_Movement>().SetupAttacker(playerPos, playerPos);

                this.enemyAttackingPips.Add(tempPip);
            }
        }

        yield return new WaitForSeconds(0.25f);   
    }

    public IEnumerator WaitUntilFinished()
    {
        yield return new WaitUntil(() => this.playerAttackingPips.Count == 0 && this.playerDefendingPips.Count == 0 && this.enemyAttackingPips.Count == 0 && this.enemyDefendingPips.Count == 0);
    }

    public void RemovePip(GameObject pip)
    {
        this.playerAttackingPips.Remove(pip);
        this.playerDefendingPips.Remove(pip);
        this.enemyAttackingPips.Remove(pip);
        this.enemyDefendingPips.Remove(pip);
    }

    private int PipAmount(float attack)
    {
        return attack switch
        {
            var a when a >= 20 => 6,
            var a when a >= 15 => 5,
            var a when a >= 10 => 4,
            var a when a >= 5 => 3,
            var a when a > 0 => 2,
            _ => 0
        };
    }
}
