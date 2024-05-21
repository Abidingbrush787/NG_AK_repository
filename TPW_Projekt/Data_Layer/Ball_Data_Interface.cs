using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer
{
    public interface IBallRepository
    {
        /// <summary>
        /// Dodaje kule do listy..
        /// </summary>
        /// <returns>NIC.</returns>
        public void AddBall(Ball ball);

        /// <summary>
        /// Usuwa kule z listy.
        /// </summary>
        /// <returns>NIC.</returns>
        public void RemoveBall(Ball ball);

        /// <summary>
        /// Dostęp do listy kul
        /// </summary>
        /// <returns>Lista wszystkich kul na planszy</returns>
        public List<Ball> GetAllBalls();

        /// <summary>
        /// Usuwa wszystkie kule
        /// </summary>
        /// <returns>NIC</returns>
        public void RemoveAllBalls();
        
    }
}
