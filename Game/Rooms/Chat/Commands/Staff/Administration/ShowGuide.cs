using Butterfly.Game.GameClients;
using System.Collections.Generic;
using System.Text;

namespace Butterfly.Game.Rooms.Chat.Commands.Cmd
                    {
                        continue;
                    }

                    if (entry.Value)
                    {
                        stringBuilder.Append("- " + guide.GetHabbo().Username + " (En service)\r");
                    }
                    else
                    {
                        stringBuilder.Append("- " + guide.GetHabbo().Username + " (Disponible)\r");
                    }
                }