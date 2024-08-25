using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.HubServices;

public interface INotificationHubService
{
    public  Task SendNotification(string key, string value);

}
