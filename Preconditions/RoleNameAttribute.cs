using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace AnotherMyouri.Preconditions
{
    public class RoleNameAttribute : PreconditionAttribute
    {
        private readonly string _name;

        // Create a constructor so the name can be specified
        public RoleNameAttribute(string name)
        {
            _name = name;
        }

        // Override the CheckPermissions method
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            // Check if this user is a Guild User, which is the only context where roles exist
            if (context.User is SocketGuildUser gUser)
            {
                // If this command was executed by a user with the appropriate role, return a success
                return Task.FromResult(gUser.Roles.Any(r => r.Name == _name) 
                    ? PreconditionResult.FromSuccess() 
                    : PreconditionResult.FromError($"You must have a role named {_name} to run this command."));
            }

            return Task.FromResult(PreconditionResult.FromError("You must be in a guild to run this command."));
        }
    }
}