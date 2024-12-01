using MediatR;

namespace OrderManagement.Common.Models
{
    public interface IResult
    {
        bool Success { get; }
        List<string> Errors { get; }
    }

    public class Result<T> : IResult
    {
        public T? Data { get; private set; } // I dati del tipo T (Order in questo caso)
        public List<string> Errors { get; private set; } // Lista di messaggi di errore
        public bool Success => !Errors.Any(); // Determina se l'operazione ha avuto successo

        // Costruttore per il successo
        public Result(T data)
        {
            Data = data;
            Errors = new List<string>();
        }

        // Costruttore per l'errore
        public Result(List<string> errors)
        {
            Data = default;
            Errors = errors ?? new List<string>();
        }

        // Metodo per aggiungere un errore
        public void AddError(string error)
        {
            Errors.Add(error);
        }

        // Metodo per restituire un risultato di successo
        public static Result<T> SuccessResult(T data) => new Result<T>(data);

        // Metodo per restituire un risultato di errore
        public static Result<T> FailureResult(List<string> errors) => new Result<T>(errors);

        // Aggiungi un metodo specifico per il caso Unit
        public static Result<Unit> SuccessResultUnit() => new Result<Unit>(Unit.Value);
    }
}
