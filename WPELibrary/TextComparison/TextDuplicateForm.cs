using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using WPELibrary.Lib;

namespace WPELibrary.TextComparison
{
    public partial class TextDuplicateForm : Form
    {
        private string Text_A = string.Empty;
        private string Text_B = string.Empty;

        #region//窗体事件

        public TextDuplicateForm(string TextA, string TextB)
        {
            InitializeComponent();

            this.Text_A = TextA;
            this.Text_B = TextB;
            this.rtbA.Text = TextA;
            this.rtbB.Text = TextB;
        }

        private void TextDuplicateForm_Load(object sender, EventArgs e)
        {
            Socket_Cache.System.IsShow_TextDuplicate = true;
        }

        private void TextDuplicateForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Socket_Cache.System.IsShow_TextDuplicate = false;
        }

        private void rtbA_TextChanged(object sender, EventArgs e)
        {
            this.lA.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_33), rtbA.Text.Length);
        }

        private void rtbB_TextChanged(object sender, EventArgs e)
        {
            this.lB.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_34), rtbB.Text.Length);
        }

        #endregion

        #region//查找

        private void bSearch_Click(object sender, EventArgs e)
        {
            try
            {
                int minBytes = (int)nudMinLength.Value;
                var results = ComparePackets(rtbA.Text.Trim(), rtbB.Text.Trim(), minBytes);

                rtbA.Text = FormatHex(results.TextA);
                rtbB.Text = FormatHex(results.TextB);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//还原

        private void bRestore_Click(object sender, EventArgs e)
        {
            this.rtbA.Text = this.Text_A;
            this.rtbB.Text = this.Text_B;
        }

        #endregion

        #region//文本查重

        private (string TextA, string TextB) ComparePackets(string stringA, string stringB, int minBytes)
        {
            stringA = CleanAndNormalizeHex(stringA);
            stringB = CleanAndNormalizeHex(stringB);

            List<string> bytes1 = SplitIntoBytes(stringA);
            List<string> bytes2 = SplitIntoBytes(stringB);

            var commonSequences = FindCommonSequences(bytes1, bytes2, minBytes);

            char[] result1 = new char[stringA.Length];
            char[] result2 = new char[stringB.Length];

            for (int i = 0; i < result1.Length; i++) result1[i] = '_';
            for (int i = 0; i < result2.Length; i++) result2[i] = '_';

            foreach (var seq in commonSequences)
            {
                for (int i = 0; i < seq.Length; i++)
                {
                    int pos = seq.Pos1 * 2 + i * 2;
                    if (pos + 1 < result1.Length)
                    {
                        result1[pos] = stringA[pos];
                        result1[pos + 1] = stringA[pos + 1];
                    }
                }

                for (int i = 0; i < seq.Length; i++)
                {
                    int pos = seq.Pos2 * 2 + i * 2;
                    if (pos + 1 < result2.Length)
                    {
                        result2[pos] = stringB[pos];
                        result2[pos + 1] = stringB[pos + 1];
                    }
                }
            }

            return (new string(result1), new string(result2));
        }

        private string FormatHex(string hex)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hex.Length; i++)
            {
                sb.Append(hex[i]);

                if (i % 2 == 1 && i != hex.Length - 1)
                {
                    sb.Append(" ");
                }                    
            }

            return sb.ToString();
        }

        private List<string> SplitIntoBytes(string hex)
        {
            List<string> bytes = new List<string>();

            for (int i = 0; i < hex.Length; i += 2)
            {
                if (i + 1 < hex.Length)
                {
                    bytes.Add(hex.Substring(i, 2));
                }
                else
                {
                    bytes.Add(hex[i] + "0");
                }                    
            }

            return bytes;
        }

        private List<(int Pos1, int Pos2, int Length)> FindCommonSequences(List<string> bytes1, List<string> bytes2, int minLength)
        {
            var result = new List<(int, int, int)>();

            for (int i = 0; i < bytes1.Count; i++)
            {
                for (int j = 0; j < bytes2.Count; j++)
                {
                    if (bytes1[i] == bytes2[j])
                    {
                        int matchLen = 1;
                        while (i + matchLen < bytes1.Count && j + matchLen < bytes2.Count && bytes1[i + matchLen] == bytes2[j + matchLen])
                        {
                            matchLen++;
                        }

                        if (matchLen >= minLength)
                        {
                            result.Add((i, j, matchLen));

                            i += matchLen - 1;
                            j += matchLen - 1;

                            break;
                        }
                    }
                }
            }

            return result;
        }

        private string CleanAndNormalizeHex(string input)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsDigit(c))
                {
                    sb.Append(c);
                }
                else if (char.ToUpper(c) >= 'A' && char.ToUpper(c) <= 'F')
                {
                    sb.Append(char.ToUpper(c));
                } 
            }

            return sb.ToString();
        }

        #endregion
    }
}
