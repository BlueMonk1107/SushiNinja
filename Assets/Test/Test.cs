using System;
using System.Text;

public class Test 
{

    public string Start(int[] array, int n)
    {
        StringBuilder s = new StringBuilder();
        Random rd = new Random();
        int[] a = new int[array.Length];     //用数组a表示该数有没有被取过，取过为1。  
        for (int i = 0; i < n; i++)
        {
            int num = rd.Next(array.Length - 1);
            if (a[num] == 0)
            {
                s.Append(array[num]);
                if (i <= n - 2)
                {
                    s.Append(",");
                }
                a[num] = 1;
            }
            else
            {
                i--;
                continue;
            }
        }
        return s.ToString();
    }

    void ASD()
    {
        string a = "s";
    }
}

