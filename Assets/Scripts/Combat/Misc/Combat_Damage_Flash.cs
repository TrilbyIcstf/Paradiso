using UnityEngine;

public class Combat_Damage_Flash : MonoBehaviour
{
    private const float OpacityGoal = 1.0f;
    private const float FlashSpeed = 15.0f;

    private float flashOpacity = 0.0f;
    private bool flash = false;

    [SerializeField]
    private SpriteRenderer sr;

    private void Update()
    {
        float time = Time.deltaTime;

        if (this.flash)
        {
            this.flashOpacity += time * FlashSpeed;

            Color spriteColor = this.sr.color;
            spriteColor.a = this.flashOpacity;
            this.sr.color = spriteColor;

            if (this.flashOpacity >= OpacityGoal)
            {
                this.flash = false;
            }
        } 
        else if (this.flashOpacity > 0)
        {
            this.flashOpacity -= time * FlashSpeed;

            Color spriteColor = this.sr.color;
            spriteColor.a = this.flashOpacity;
            this.sr.color = spriteColor;
        }
    }

    public void FlashOn()
    {
        this.flash = true;
    }
}
