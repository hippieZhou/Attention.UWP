using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Attention.Core.Commands
{
    public abstract class Command<TOUTPUT>
    {
        public DateTime Timestamp { get; private set; }
        protected Command() => Timestamp = DateTime.Now;
    }

    public interface ICommandHandler<in TREQUEST, TRESPONSE> where TREQUEST : Command<TRESPONSE>
    {
        Task<TRESPONSE> Handle(TREQUEST request, CancellationToken cancellationToken);
    }

    public class Response
    {
        protected Response(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors ?? Enumerable.Empty<string>();
        }

        public bool Succeeded { get; private set; }
        public IEnumerable<string> Errors { get; private set; }

        public static Response Success() => new Response(true, Enumerable.Empty<string>());

        public static Response Failure(IEnumerable<string> errors) => new Response(false, errors);
    }

    public class Response<DTO> : Response
    {
        protected Response(DTO data) : base(true, Enumerable.Empty<string>())
        {
            Data = data;
        }

        protected Response(IEnumerable<string> errors) : base(false, errors)
        {
        }

        public DTO Data { get; private set; }

        public static Response<DTO> Success(DTO data) => new Response<DTO>(data);
    }
}
