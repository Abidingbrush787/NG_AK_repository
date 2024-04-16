using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using Data_Layer;


namespace Logic_Layer.Interfaces
{
    public interface IBallService
    {
        /// <summary>
        /// Tworzy i zwraca nową kulkę z losowymi właściwościami.
        /// </summary>
        /// <returns>Nowo utworzona kula.</returns>
        Ball CreateBall();

        /// <summary>
        /// Aktualizuje pozycje wszystkich kulek w zależności od czynnika czasu.
        /// </summary>
        /// <param name="timeFactor">Czynnik czasu wpływający na przesunięcie.</param>
        void UpdateBallPositions(double timeFactor);

        /// <summary>
        /// Usuwa wszystkie kulki z kolekcji.
        /// </summary>
        void ClearBalls();

        /// <summary>
        /// Zwraca kolekcję wszystkich kulek.
        /// </summary>
        /// <returns>Kolekcja kulek.</returns>
        IEnumerable<Ball> GetAllBalls();
    }
}

 