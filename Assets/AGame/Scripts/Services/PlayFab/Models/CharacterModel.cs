public class CharacterModel
{
    private string _name;
    private int _level;
    private int _damage;
    private int _gold;
    private int _health;
    private int _experience;

    public string Name { get { return _name; } private set { _name = value; } }
    public int Damage { get { return _damage; } set { _damage = value; } }
    public int Level { get { return _level; } set { _level = value; } }
    public int Gold { get { return _gold; } set { _gold = value; } }
    public int Health { get { return _health; } set { _health = value; } }
    public int Experience { get { return _experience; } set { _experience = value; } }

    public CharacterModel(string name)
    {
        Name = name;
        Level = 1;
        Damage = 1;
        Gold = 0;
        Health = 100;
        Experience = 0;
    }

    public CharacterModel(string name, int level, int damage, int health, int experience)
    {
        Name = name;
        Level = level;
        Damage = damage;
        Gold = 0;
        Health = health;
        Experience = experience;
    }
}
