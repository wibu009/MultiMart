using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Common.Exporters;

public interface IExcelWriter : ITransientService
{
    Stream WriteToStream<T>(IList<T> data);
}