using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JacksOrBetter
{
    public class CommandsPoker
    {
        private static RoutedUICommand agregar;
        private static RoutedUICommand reparteix;
        private static RoutedUICommand duplicar;
        private static RoutedUICommand semiduplicar;

        static CommandsPoker()
        {
            reparteix = new RoutedUICommand("Reparteix", "cmdReparteix", typeof(CommandsPoker));
            duplicar = new RoutedUICommand("Duplicar", "cmdDuplicar", typeof(CommandsPoker));
            semiduplicar = new RoutedUICommand("Semiduplicar", "cmdSemiduplicar", typeof(CommandsPoker));
            agregar = new RoutedUICommand("Agregar", "cmdAgregar", typeof(CommandsPoker));
        }
        public static RoutedUICommand Agregar { get => agregar; }
        public static RoutedUICommand Reparteix { get => reparteix; }
        public static RoutedUICommand Duplicar { get => duplicar; }
        public static RoutedUICommand Semiduplicar { get => semiduplicar; }

    }
}
