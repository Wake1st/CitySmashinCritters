public class AttackProfile
{
  public AttackType lightPunch;
  public AttackType lightKick;
  public AttackType lightSpecial;
  public AttackType heavyPunch;
  public AttackType heavyKick;
  public AttackType heavySpecial;

  public AttackType[] attackTypes;

  public AttackProfile()
  {
    lightPunch = new AttackType(0, 'l', 6, "SoundEffects/Punch");
    lightKick = new AttackType(1, 'k', 8, "SoundEffects/Kick");
    lightSpecial = new AttackType(2, 'j', 12, "SoundEffects/Special");
    heavyPunch = new AttackType(3, 'o', 10, "SoundEffects/Punch", 0.8f);
    heavyKick = new AttackType(4, 'i', 14, "SoundEffects/Kick", 0.8f);
    heavySpecial = new AttackType(5, 'u', 20, "SoundEffects/Special", 1.2f);

    attackTypes = new AttackType[] {
      lightPunch,
      lightKick,
      lightSpecial,
      heavyPunch,
      heavyKick,
      heavySpecial,
    };
  }
}