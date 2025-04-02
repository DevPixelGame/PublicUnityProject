using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System;
using System.Text.RegularExpressions;

public class CalClass
{
    public static int CalBackToWave(int currentWaveIndex)
    {
        int backToWaveIndex = 0;
        int dividedWave = currentWaveIndex % 10;
        // 1 ~ 5 >>> 1
        // 11 ~ 15 >> 11
        
        // 6 ~ 10 >>> 5
        // 16 ~ 20 >>> 15
        if (dividedWave == 1)
        {
            backToWaveIndex = currentWaveIndex;
        } else if (dividedWave == 2)
        {
            backToWaveIndex = currentWaveIndex - 1;
        } else if (dividedWave == 3)
        {
            backToWaveIndex = currentWaveIndex - 2;
        } else if (dividedWave == 4)
        {
            backToWaveIndex = currentWaveIndex - 3;
        } else if (dividedWave == 5)
        {
            backToWaveIndex = currentWaveIndex - 4;
        }
        else if (dividedWave == 6)
        {
            backToWaveIndex = currentWaveIndex - 5;
        }
        else if (dividedWave == 7)
        {
            backToWaveIndex = currentWaveIndex - 6;
        }
        else if (dividedWave == 8)
        {
            backToWaveIndex = currentWaveIndex - 7;
        }
        else if (dividedWave == 9)
        {
            backToWaveIndex = currentWaveIndex - 8;
        }
        else if (dividedWave == 0) // 보스
        {
            backToWaveIndex = currentWaveIndex - 9;
        }
        
        return backToWaveIndex;
    }

    public static Color GetInActiveColor(bool isActive = true)
    {
        Color newColor = Color.white;
        if (!isActive)
        {
            if (ColorUtility.TryParseHtmlString("#7B7B7BFF", out newColor))
            {
                return newColor;
            }
        }

        return newColor;
    }

    public static string GetColorByRanking(string str)
    {
        string newColor = "#d1d1d1";
        if (!str.Equals("[-위]") && !str.IsNullOrEmpty())
        {
            string numstring = Regex.Replace(str, @"\D", "");
            int rrr = int.Parse(numstring);

            if (rrr == 1) // 1위
            {
                return "#ff0000";
            } else if (rrr == 2) // 2위
            {
                return "#ffe70d";
            } else if (rrr == 3) // 3위 
            {
                return "#3672ff";
            } else if (rrr > 3 && rrr <= 100)
            {
                return "#a07ed6";
            }
            else
            {
                return "#b3b3b3";
            }
        }

        return newColor;
    }

    public static Color GetColorByClassTypeFireHell()
    {
        Color newColor = Color.white;
        if (ColorUtility.TryParseHtmlString("#D20500", out newColor))
        {
            return newColor;
        }
        
        return newColor;
    }
    
    public static Color GetColorByClassType(int index)
    {
        Color newColor = Color.white;

        if (index == 0) // 일반
        {
            if (ColorUtility.TryParseHtmlString("#F1F1F1", out newColor))
            {
                return newColor;
            }
        }        
        else if (index == 1) // 고급
        {
            if (ColorUtility.TryParseHtmlString("#11FF00", out newColor))
            {
                return newColor;
            }
        }
        else if (index == 2) // 희귀
        {
            if (ColorUtility.TryParseHtmlString("#00E9FF", out newColor))
            {
                return newColor;
            }
        } else if (index == 3) // 영웅
        {
            if (ColorUtility.TryParseHtmlString("#FFAD00", out newColor))
            {
                return newColor;
            }
        } else if (index == 4) // 전설
        {
            if (ColorUtility.TryParseHtmlString("#FF1100", out newColor))
            {
                return newColor;
            }
        }
        
        return newColor;
    }
    
    
    public static Color GetColorByClassTypeAltar(int index)
    {
        Color newColor = Color.white;

        if (index == 0) // 일반
        {
            if (ColorUtility.TryParseHtmlString("#F1F1F1", out newColor))
            {
                return newColor;
            }
        }        
        else if (index == 1) // 고급
        {
            if (ColorUtility.TryParseHtmlString("#00FF28", out newColor))
            {
                return newColor;
            }
        }
        else if (index == 2) // 희귀
        {
            if (ColorUtility.TryParseHtmlString("#00EAFF", out newColor))
            {
                return newColor;
            }
        } else if (index == 3) // 영웅
        {
            if (ColorUtility.TryParseHtmlString("#FF9043", out newColor))
            {
                return newColor;
            }
        } else if (index == 4) // 전설
        {
            if (ColorUtility.TryParseHtmlString("#FF1100", out newColor))
            {
                return newColor;
            }
        }
        
        return newColor;
    }
    
    // public static float Round(float num, int mult)
    // {
    //     int multnum = 1;
    //     for (int i = 0; i < mult; i++)
    //     {
    //         multnum *= 10;
    //     }
    //     return Mathf.Round(num * multnum) / multnum;
    // }
    //
    //
    // public static int CalcMaxExp(int level)
    // {
    //     float num = (10 * Mathf.Pow((level - 1), (0.9f + acc_a / 250)) * level * (level + 1)) /
    //         (6 + Mathf.Pow(level, 2) / 50 / acc_b) + (level - 1) * extra;
    //     if (level == 1)
    //     {
    //         num = (10 * Mathf.Pow((level - 1), (0.9f + acc_a / 250)) * level * (level + 1)) /
    //             (6 + Mathf.Pow(level, 2) / 50 / acc_b) + (level) * extra;
    //     }
    //     num = Round(num, 0);
    //     return (int)num;
    // }
    public static double Round(double num, int mult)
    {
        int multnum = 1;
        for (int i = 0; i < mult; i++)
        {
            multnum *= 10;
        }
        return num * multnum;
    }
    
    // static readonly int basis = 30;
    // static readonly int acc_a = 30;
    // static readonly int acc_b = 30;
    // static readonly int extra = 50;
    static readonly int basis = 15;
    static readonly int acc_a = 25;
    static readonly int acc_b = 25;
    static readonly int extra = 15;
    public static BigInteger CalcMaxExp(int level)
    {
        double num = (basis * Mathf.Pow((level - 1), (0.9f + acc_a / 250)) * level * (level + 1)) /
            (6 + Mathf.Pow(level, 2) / 50 / acc_b) + level * extra;

        int multiplierIndex = 0;
        if (level <= 100)
        {
            multiplierIndex = 0;
        } else if (level > 100 && level <= 200)
        {
            multiplierIndex = 1;
        } else if (level > 200 && level <= 300)
        {
            multiplierIndex = 2;
        } else if (level > 300 && level <= 400)
        {
            multiplierIndex = 3;
        } else if (level > 400 && level <= 500)
        {
            multiplierIndex = 4;
        } else if (level > 500 && level <= 600)
        {
            multiplierIndex = 5;
        } else if (level > 600 && level <= 700)
        {
            multiplierIndex = 6;
        } else if (level > 700 && level <= 800)
        {
            multiplierIndex = 7;
        } else if (level > 800 && level <= 900)
        {
            multiplierIndex = 8;
        } else if (level > 900 && level <= 1000)
        {
            multiplierIndex = 9;
        } else if (level > 1000 && level <= 1100)
        {
            multiplierIndex = 10;
        }
        
        num = Round(num, multiplierIndex);
        BigInteger res = new BigInteger(num);
        return res;
    }

    
    public static BigInteger CalIncreasedExpPercentage(BigInteger origin, float addAmount, bool isByMob = false)
    {
        // BigInt 계산시에 double로 한번 casting 한다.
        
					
        float costMultiply = 1 + (addAmount * 0.01f);
        double castedDamage = (double)origin;

        double result = castedDamage * costMultiply;

        // And since you want a BigInteger at the end
        BigInteger bigIntResult = new BigInteger(result);
					
        return bigIntResult;
    }

    // Int n%만큼 증가된 값
    public static int CalcIncreasedValue(int origin, float addAmount)
    {
        // BigInt 계산시에 double로 한번 casting 한다.
        float costMultiply = 1 + (addAmount * 0.01f);
        float result = origin * costMultiply;
        // And since you want a BigInteger at the end
					
        return (int)result;
    }    
    
    public static BigInteger CalcPercentageOfBigInt(BigInteger origin, float addAmount)
    {
        // 전체값 X 퍼센트 ÷ 100
        
        // BigInt 계산시에 double로 한번 casting 한다.
        double castedDamage = (double)origin;
        double result = (castedDamage * addAmount) * 0.01f;
        // And since you want a BigInteger at the end
        BigInteger bigIntResult = new BigInteger(result);
					
        return bigIntResult;
    }
    
    // 데미지 증가 구현,
    public static BigInteger CalIncreasedPercentDamage(BigInteger origin, float addAmount)
    {
        // BigInt 계산시에 double로 한번 casting 한다.
        if (origin <= 0)
        {
            return 0;
        }

        float costMultiply = 1 + (addAmount * 0.01f);
        double castedDamage = (double)origin;
        double result = castedDamage * costMultiply;
        // And since you want a BigInteger at the end
        BigInteger bigIntResult = new BigInteger(result);
					
        return bigIntResult;
    }
    
    public static float GetBigIntPercentage(BigInteger divide, BigInteger max) {
        float result = 0f;
        if (divide > 0)
        {
            divide *= 1000;
            divide /= max;
    
            result = (float)divide;
            result /= 1000f;
        }
        else
        {
            result = 0;
        }
    
        return result;
    }
    
    public static BigInteger CalDecreasedPercentDamage(BigInteger origin, float addAmount)
    {
        // BigInt 계산시에 double로 한번 casting 한다.
					
        float costMultiply = 1 - (addAmount * 0.01f);
        double castedDamage = (double)origin;
        double result = castedDamage * costMultiply;
        // And since you want a BigInteger at the end
        BigInteger bigIntResult = new BigInteger(result);
					
        return bigIntResult;
    }
    

    static BigInteger bbValue = BigInteger.Parse("1000000000000000000");
    static BigInteger aaValue = BigInteger.Parse("1000000000000000");
    static BigInteger tValue = BigInteger.Parse("1000000000000");
    static BigInteger bValue = BigInteger.Parse("1000000000");
    static BigInteger mValue = BigInteger.Parse("1000000");
    static BigInteger kValue = BigInteger.Parse("100000");
    // param: BigInteger
    // Convert BigInteger >>>> String 
    // public static string ConvertNumberFormat(BigInteger bigInteger)
    // {
    //     string convertedNumber = String.Empty;
    //     // 1 이면, 왼쪽이 오른쪽보다 큰거
    //     // 0 이면 같은거
    //     // -1 이면 오른쪽이 더 큰거
    //     if (BigInteger.Compare(bigInteger, bbValue) == 1 || BigInteger.Compare(bigInteger, bbValue) == 0)
    //     {
    //         // m
    //         BigInteger divider = BigInteger.Divide(bigInteger, bbValue);
    //         // double num = Math.Round(divider, 1).ToString("N1") + "T";
    //         convertedNumber = divider.ToString("N2") + "bb";
    //
    //         return convertedNumber;
    //     }
    //     
    //     if (BigInteger.Compare(bigInteger, aaValue) == 1 || BigInteger.Compare(bigInteger, aaValue) == 0)
    //     {
    //         // m
    //         BigInteger divider = BigInteger.Divide(bigInteger, aaValue);
    //         // double num = Math.Round(divider, 1).ToString("N1") + "T";
    //         convertedNumber = divider.ToString("N2") + "aa";
    //
    //         return convertedNumber;
    //     }
    //     if (BigInteger.Compare(bigInteger, tValue) == 1 || BigInteger.Compare(bigInteger, tValue) == 0)
    //     {
    //         // m
    //         BigInteger divider = BigInteger.Divide(bigInteger, tValue);
    //         // double num = Math.Round(divider, 1).ToString("N1") + "T";
    //         convertedNumber = divider.ToString("N2") + "T";
    //
    //         return convertedNumber;
    //     }
    //     if (BigInteger.Compare(bigInteger, bValue) == 1 || BigInteger.Compare(bigInteger, bValue) == 0)
    //     {
    //         // m
    //         BigInteger divider = BigInteger.Divide(bigInteger, bValue);
    //         // double num = Math.Round(divider, 1).ToString("N1") + "T";
    //         convertedNumber = divider.ToString("N2") + "B";
    //
    //         return convertedNumber;
    //     }
    //     if (BigInteger.Compare(bigInteger, mValue) == 1 || BigInteger.Compare(bigInteger, mValue) == 0)
    //     {
    //         BigInteger divider = BigInteger.Divide(bigInteger, mValue);
    //         // double num = Math.Round(divider, 1).ToString("N1") + "T";
    //         convertedNumber = divider.ToString("N2") + "M";
    //
    //         return convertedNumber;
    //     }
    //     if (BigInteger.Compare(bigInteger, kValue) > 0)
    //     {
    //         Debug.Log("**** bigInteger : " + bigInteger);
    //         BigInteger divider = BigInteger.Divide(bigInteger, kValue);
    //         Debug.Log("**** bigInteger divider : " + divider);
    //         // double num = Math.Round(divider, 1).ToString("N1") + "T";
    //         convertedNumber = divider.ToString("F1") + "K";
    //         Debug.Log("**** bigInteger convertedNumber : " + convertedNumber);
    //         return convertedNumber;
    //     }
    //     
    //     if (BigInteger.Compare(bigInteger, kValue) == -1)
    //     {
    //         convertedNumber = bigInteger.ToString();
    //         
    //         
    //         return convertedNumber;
    //     }
    //     
    //     
    //     return convertedNumber;
    // }
    //
    
    
    
    
    
    
    
    
    static readonly string[] CurrencyUnits = new string[] { "", "K", "M", "B", "T", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", "ca", "cb", "cc", "cd", "ce", "cf", "cg", "ch", "ci", "cj", "ck", "cl", "cm", "cn", "co", "cp", "cq", "cr", "cs", "ct", "cu", "cv", "cw", "cx", };

    /// <summary>
    /// double 형 데이터를 클리커 게임의 화폐 단위로 표현
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static string ConvertNumberFormat(BigInteger bigNumber)
    {
        double number = (double)bigNumber;
        
        string zero = "0";

        if (-1d < number && number < 1d)
        {
            return zero;
        }

        if (double.IsInfinity(number))
        {
            return "Infinity";
        }

        //  부호 출력 문자열
        string significant = (number < 0) ? "-" : string.Empty;

        //  보여줄 숫자
        string showNumber = string.Empty;

        //  단위 문자열
        string unityString = string.Empty;

        //  패턴을 단순화 시키기 위해 무조건 지수 표현식으로 변경한 후 처리
        string[] partsSplit = number.ToString("E").Split('+');

        //  예외
        if (partsSplit.Length < 2)
        {
            return zero;
        }

        //  지수 (자릿수 표현)
        if (!int.TryParse(partsSplit[1], out int exponent))
        {
            // Debug.LogWarningFormat("Failed - ToCurrentString({0}) : partSplit[1] = {1}", number, partsSplit[1]);
            return zero;
        }

        //  몫은 문자열 인덱스
        int quotient = exponent / 3;

        //  나머지는 정수부 자릿수 계산에 사용(10의 거듭제곱을 사용)
        int remainder = exponent % 3;

        //  1A 미만은 그냥 표현
                          //  87237
                          // 100000
        if (exponent < 5) // 1000
        {
            quotient = 0;
            showNumber = System.Math.Truncate(number).ToString();
        }
        else
        {
            //  10의 거듭제곱을 구해서 자릿수 표현값을 만들어 준다.
            var temp = double.Parse(partsSplit[0].Replace("E", "")) * System.Math.Pow(10, remainder);

            //  소수 둘째자리까지만 출력한다.
            showNumber = temp.ToString("F").Replace(".00", "");
        }

        unityString = CurrencyUnits[quotient];
        
        return string.Format("{0}{1}{2}", significant, showNumber, unityString);
    }

    
    public static string ConvertRaidScore(BigInteger bigNumber)
    {
        if (bigNumber <= 0)
        {
            return "";
        }

        string num = string.Format("{0:# #### #### #### #### #### #### #### #### ####}", bigNumber).TrimStart().Replace(" ", ",");

        string[] unit = new string[] { "", "만", "억", "조", "경", "해", "자", "양", "구", "간"};
        string[] str = num.Split(',');
        string result = "";
        int cnt = 0;
        for (int i = str.Length; i > 0; i--)
        {
            if (Convert.ToInt32(str[i - 1]) != 0)
            {
                result = Convert.ToInt32(str[i - 1]) + unit[cnt] + result;
            }
            cnt++;
        }
        return result;
    }













    // public static T DeepCopy<T>(T obj)
    // {
    //     using (var stream = new MemoryStream())
    //     {
    //         var formatter = new BinaryFormatter();
    //         formatter.Serialize(stream, obj);
    //         stream.Position = 0;
    //
    //         return (T)formatter.Deserialize(stream);
    //     }
    // }


    // public static int GetGoldRewardByWaveLevel(int waveIndex)
    // {
    //     return 1000 * waveIndex;
    // }

    // public static int GetGoldRewardByWaveLevel(int waveIndex)
    // {
    //     return 1000 * waveIndex;
    // }

    // public static int GetCristalRewardByWaveLevel(int waveIndex)
    // {
    //     return 1 * waveIndex;
    // }

    // public static int GetExpRewardByWaveLevel(int waveIndex)
    // {
    //     return 100 * waveIndex;
    // }

    // public static int GetExpByPlayerLevel(int level)
    // {
    //     return 100 * level;
    // }

    public float Abs(float res)
    {
        if (res > 0)
        {
            return res;
        }

        return res * -1;
    }
    
    
    // 초항, 등차, 레벨
    public static BigInteger SumFomula(BigInteger basisA, int basisDD, int n)
    {
        if (n > 1)
        {
            n -= 1;
        }

        BigInteger sum =  n * ((2* basisA) + ((n-1) * basisDD));
        double newSum = (double)sum / 2;
        
        BigInteger bigIntegerRes = new BigInteger(newSum);
        
        return bigIntegerRes + basisA;
    }
    
    // 계차 합
    public static BigInteger SumFomula_02(BigInteger basisA, int basisDD, int n)
    {
        if (n > 1)
        {
            n -= 1;
        }
        
        BigInteger sum =  n * ((2* basisA) + ((n-1) * basisDD));
        double newSum = (double)sum * 0.5f;
        
        BigInteger bigIntegerRes = new BigInteger(newSum);
        
        // Debug.Log("**** SumFomula : " + (bigIntegerRes) + "/// n: " + n);
        return bigIntegerRes;
    }
}