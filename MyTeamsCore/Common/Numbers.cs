using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTeamsCore.Common;

public static class Numbers {
    
    public static int DefaultDoubleDeviationDigitsCount = (int)Math.Log(DefaultDoubleDeviation, 0.1);
    public const double DefaultDoubleDeviation = 0.000001;
    public const float DefaultFloatDeviation = 0.000001f;

    public static bool
    IsZero(this double value, double deviation = DefaultDoubleDeviation) => Math.Abs(value) < deviation;

    public static bool
    IsZero(this float value, float deviation = DefaultFloatDeviation) => Math.Abs(value) < deviation;

    public static bool
    IsNotZero(this double value, double deviation = DefaultDoubleDeviation) => !value.IsZero(deviation);

    public static bool
    IsEqual(this double value, double other, double deviation = DefaultDoubleDeviation) {
        if (value == other || double.IsNaN(value) && double.IsNaN(other))
            return true;
            
        return Math.Abs(value -  other)  < deviation;  
    } 

    public static bool
    IsNotEqual(this double value, double other, double deviation = DefaultDoubleDeviation) => !value.IsEqual(other, deviation);

    public static bool
    IsGreater(this double value, double other, double deviation = DefaultDoubleDeviation) => value - other > deviation;

    public static bool
    IsLess(this double value, double other, double deviation = DefaultDoubleDeviation) => other - value > deviation;

    public static bool
    IsLessMaxValue(this double value) => value.IsLess(double.MaxValue);

    public static bool
    IsGreaterOrEqual(this double value, double other, double deviation = DefaultDoubleDeviation) => value - other > -deviation;

    public static bool
    IsLessOrEqual(this double value, double other, double deviation = DefaultDoubleDeviation) => other - value > -deviation;

    public static bool
    IsLessOrEqual(this float value, float other, double deviation = DefaultDoubleDeviation) => other - value > -deviation;

    public static bool 
    IsLessOrEqualZero(this double value, double deviation = DefaultDoubleDeviation) => value.IsLessOrEqual(0, deviation);

    public static bool
    IsGreaterZero(this double value, double deviation = DefaultDoubleDeviation) => value > deviation;

    public static bool
    IsLessZero(this double value, double deviation = DefaultDoubleDeviation) => value < -deviation;

    public static bool
    IsMaxValue(this double value) => double.IsInfinity(value) || value == double.MaxValue;
    
    public static bool 
    IsMinValue(this double value) => double.IsNegativeInfinity(value) || value == double.MinValue;

    public static double
    VerifyPositive(this double value, string? name = null) {
        if (value.IsGreaterZero())
            return value;

        throw new InvalidOperationException($"Expecting positive value {name} but was {value}");
    }

    public static double
    VerifyGreaterOrEqualZero(this double value, string? name = null) {
        if (value.IsLessZero())
            throw new InvalidOperationException($"Expecting value {name} to be greater or equal zero but was {value}");

        return value;
    }

    public static ushort
    VerifyArgumentPositive(this ushort value, string? argumentName) {
        if (value <= 0)
            throw new ArgumentException(argumentName, $"Expecting positive value but was ({value})");
        return value;
    }

    public static long
    VerifyArgumentGreater(this long value, long other, string? argumentName = null) {
        if (value <= other)
            throw new ArgumentException(argumentName, $"Expecting value ({value}) greater than other ({other})");
        return value;
    }

    public static long
    VerifyPositive(this long value, string? name = null) =>
        value > 0 ? value : throw new InvalidOperationException($"Value {name} must be positive but was {value}");

    public static long
    VerifyArgumentNotNegative(this long value, string? name = null) =>
        value >= 0 ? value : throw new ArgumentOutOfRangeException($"Value can't be negative but was {value}", name);

    public static int
    VerifyPositive(this int value, string? name = null) =>
        value > 0 ? value : throw new InvalidOperationException($"Value {name} must be positive but was {value}");

    public static double
    VerifyArgumentNotNegative(this double value, string? name = null) {
        if (value.IsLessZero())
            throw new ArgumentOutOfRangeException($"Value can't be negative but was {value}", name);
        return value;
    }
    
    public static int
    VerifyArgumentNotNegative(this int value, string? name = null) {
        if (value < 0)
            throw new ArgumentOutOfRangeException($"Value can't be negative but was {value}", name);
        return value;
    }

    public static int 
    VerifyArgumentNotZero(this int value, string? argumentName = null) {
        if (value == 0) throw new ArgumentException("Value can't be zero", nameof(argumentName));
        return value;
    }

    public static int
    VerifyArgumentPositive(this int value, string? name = null) {
        if (value <= 0)
            throw new ArgumentOutOfRangeException($"Value must be positive but was {value}", name);
        return value;
    }

    public static double
    VerifyArgumentPositive(this double value, string? name = null) {
        if (value.IsLessOrEqual(0))
            throw new ArgumentOutOfRangeException($"Value must be positive but was {value}", name);
        return value;
    }
    
    public static double 
    VerifyArgumentIsZero(this double value, string? name = null) {
        if (!value.IsZero())
            throw new ArgumentException($"Expecting a zero value but was {value}", name);
        return value;
    }

    public static double
    VerifyArgumentRoundedToCents(this double value, string? name = null) {
        if (!((value*100).RoundDeviation()).IsEqual(Math.Floor((value*100).RoundDeviation())))
            throw new ArgumentException($"Expecting value {name} to be rounded to cents but was {value}");
        return value;
    }

    public static double
    RoundDeviation(this double value) => Math.Round(value, DefaultDoubleDeviationDigitsCount + 1);    

    public static double
    RoundAmount(this double value, int currencyExponent) => value.IsNaN() ? double.NaN : Math.Round(value, currencyExponent, MidpointRounding.AwayFromZero);
    
    public static double 
    Rounded(this double value, int digits = 0) => Math.Round(value, digits);

    public static double
    VerifyRoundedToDeviation(this double value, string? name = null, double deviation = DefaultDoubleDeviation) {
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (value.RoundDeviation() != value)
            throw new InvalidOperationException($"Expecting {name} to be rounded to deviation but was {value}");
        return value;
    }

    public static double
    VerifyArgumentNotNan(this double value, string? name = null) {
        if (double.IsNaN(value))
            throw new ArgumentException("Can't be NaN", name);
        return value;
    }

    public static bool 
    IsPowerOfTwo(this int x) =>
        (x != 0) && ((x & (x - 1)) == 0);
    
    public static bool
    IsNaN(this double value) => double.IsNaN(value);

    public static double
    Squared(this double value) => Math.Pow(value, 2);

}