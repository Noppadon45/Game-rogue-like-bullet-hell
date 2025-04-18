using UnityEngine;

public class AuraWeapon : Weapon
{

    protected Aura currentAura;

    // Update is called once per frame
    protected override void Update() { }

    // Called when the weapon is equipped
    public override void OnEquip()
    {
        // Try to replace the aura the weapon has with a new one.
        if (currentStats.auraPrefab)
        {
            if (currentAura) Destroy(currentAura);
            currentAura = Instantiate(currentStats.auraPrefab, transform);
            currentAura.weapon = this;
            currentAura.player = playerStats;

            // Scale aura based on current area stat
            currentAura.transform.localScale = new Vector3(currentStats.area, currentStats.area, currentStats.area);
        }
    }

    // Remove the aura when unequipped
    public override void OnUnEquip()
    {
        if (currentAura) Destroy(currentAura);
    }

    // Leveling up increases area; update aura if it's active
    public override bool DoLevelUp()
    {
        if (!base.DoLevelUp()) return false;

        // If there is an aura attached to this weapon, we update the aura.
        if (currentAura)
        {
            currentAura.transform.localScale = new Vector3(currentStats.area, currentStats.area, currentStats.area);
        }
        return true;
    }

}