using Butterfly.HabboHotel.GameClients;
using System.Collections.Generic;
using System.Text;

namespace Butterfly.HabboHotel.Rooms.Chat.Commands.Cmd
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