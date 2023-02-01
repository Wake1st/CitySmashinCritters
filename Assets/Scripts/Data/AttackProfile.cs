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
    lightPunch = new AttackType(0, 'l', 6, "SoundEffects/Punch", 1, 0.2f);
    lightKick = new AttackType(1, 'k', 8, "SoundEffects/Kick", 1, 0.3f);
    lightSpecial = new AttackType(2, 'j', 42, "SoundEffects/Special", 1, 2f);
    heavyPunch = new AttackType(3, 'o', 10, "SoundEffects/Punch", 0.8f, 0.6f);
    heavyKick = new AttackType(4, 'i', 14, "SoundEffects/Kick", 0.8f, 0.8f);
    heavySpecial = new AttackType(5, 'u', 100, "SoundEffects/Special", 1.2f, 4f);

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