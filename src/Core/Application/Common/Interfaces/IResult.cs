using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Common.Interfaces;
public interface IResult
{
    string[] Errors { get; set; }

    bool Succeeded { get; set; }
}
public interface IResult<out T> : IResult
{
    T Data { get; }
}
public interface IResult<out T, out T1> : IResult
{
    T Data { get; }
    T1 Data1 { get; }
}
