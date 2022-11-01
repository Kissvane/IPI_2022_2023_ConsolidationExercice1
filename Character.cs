using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrectionConsolidation
{
    public enum CharacterClass { ARCHER = 0, WARRIOR = 1, HERO = 2, WIZARD = 3 };


    public class Character
    {
        public string Name { get; set; }
        public int Strength { get; set; }
        public int Speed { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }

        public override string ToString()
        {
            return $"{Name} {Strength} {Speed} {Wisdom} {Charisma}";
        }
    }

    public class ClassedCharacter : Character
    {
        public CharacterClass Class;

        public ClassedCharacter(Character character)
        {
            this.Name = character.Name;
            this.Speed = character.Speed;
            this.Strength = character.Strength;
            this.Wisdom = character.Wisdom;
            this.Charisma = character.Charisma;

            int max = CalculateMaximumStat();
            if (Speed == max)
                Class = CharacterClass.ARCHER;
            else if (Wisdom == max)
                Class = CharacterClass.WIZARD;
            else if (Strength == max)
                Class = CharacterClass.WARRIOR;
            else
                Class = CharacterClass.HERO;
        }

        public override string ToString()
        {
            return string.Concat($" {Class} ",base.ToString());
        }

        public int CalculateMaximumStat()
        {
            return Math.Max(Math.Max(Math.Max(Strength, Speed), Charisma), Wisdom);
        }

    }
}
