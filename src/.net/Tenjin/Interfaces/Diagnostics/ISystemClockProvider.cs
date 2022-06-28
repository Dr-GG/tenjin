using System;

namespace Tenjin.Interfaces.Diagnostics;

public interface ISystemClockProvider
{
    DateTime Now();
}