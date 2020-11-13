using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    class Racers
    {
        void UpdateRacers(float deltaTimeS, List<Racer> racers)
        {
            List<Racer> racersNeedingRemoved = new List<Racer>();
            racersNeedingRemoved.Clear();// Updates the racers that are aliveintracer
            
            for (int racerIndex = 0; racerIndex < racers.Count; racerIndex++)
            {
                if (racers[racerIndex].IsAlive())
                {
                    //Racer update takes milliseconds
                    racers[racerIndex].Update(deltaTimeS * 1000.0f);
                }

                // Collides
                for (int racerIndex2 = 0; racerIndex2 < racers.Count; racerIndex2++)
                {
                    Racer racer1 = racers[racerIndex];
                    Racer racer2 = racers[racerIndex2];
                    if (racerIndex != racerIndex2)
                    {
                        if (racer1.IsCollidable() && racer2.IsCollidable() && racer1.CollidesWith(racer2))
                        {
                            OnRacerExplodes(racer1);
                            racersNeedingRemoved.Add(racer1);
                            racersNeedingRemoved.Add(racer2);

                            //remove the exploded vehicles from the racers list, to avoid looping again to find who is not exploded.
                            //so the items left in racers are the new racers list.
                            racers.Remove(racers[racerIndex]);
                        }
                    }
                }
            }
        }

    }
}
