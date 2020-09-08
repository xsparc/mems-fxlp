using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace mems_fx3lp
{
    public class EllipFit
    {
        public static double getFit()
        {


            double[] x_old = { 60, 60, 57, 58, 61, 58, 63, 66, 67, 53, 63, 63, 65, 59, 60, 55, 56, 58, 60, 60 };
            double[] y_old = { 20, 19, 17, 18, 14, 10, 21, 23, 21, 35, 32, 31, 33, 34, 34, 41, 41, 43, 40, 38 };
            double[] z_old = { 247, 259, 258, 260, 252, 252, 248, 255, 243, 254, 251, 241, 256, 254, 247, 250, 252, 245, 246, 246 };
            int M_order = x_old.Length;
            int N_order = 9;

            double[] xSquared = vectSquare(x_old);
            double[M_order][N_order] D_data;
            double[M_order][M_order] mD;
            double[N_order][M_order] DTrans;

            for (int i = 0; i < M_Order; i++)
            {
                D_data[i][0] = x_old[i] * x_old[i];
                D_data[i][1] = y_old[i] * y_old[i];
                D_data[i][2] = z_old[i] * z_old[i];
                D_data[i][3] = 2 * x_old[i] * y_old[i];
                D_data[i][4] = 2 * x_old[i] * z_old[i];
                D_data[i][5] = 2 * y_old[i] * z_old[i];
                D_data[i][6] = 2 * x_old[i];
                D_data[i][7] = 2 * y_old[i];
                D_data[i][8] = 2 * z_old[i];
            }

        }
        int MAT_MAX_SIZE = 100;

        public double getCofactor(double[MAT_MAX_SIZE][MAT_MAX_SIZE] A, double[MAT_MAX_SIZE][MAT_MAX_SIZE] temp, int p, int q, int n)
        {
            int i = 0, j = 0;

            for (int row = 0; row < n; row++)
            {
                for (int col = 0; col < n; col++)
                {
                    // Copying into temporary matrix only those element 
                    // which are not in given row and column 
                    if (row != p && col != q)
                    {
                        temp[i][j++] = A[row][col];

                        // Row is filled, so increase row index and 
                        // reset col index 
                        if (j == n - 1)
                        {
                            j = 0;
                            i++;
                        }
                    }
                }
            }

        }

        public double determinant(double[MAT_MAX_SIZE][MAT_MAX_SIZE] A, int n)
        {
            int determinant_res = 0; // Initialize result 

            // Base case : if matrix contains single element 
            if (n == 1)
                return (int)A[0][0];

            double[MATRIX_MAX_SIZE][MATRIX_MAX_SIZE] temp; // To store cofactors 

            int sign = 1; // To store sign multiplier 

            // Iterate for each element of first row 
            for (int f = 0; f < n; f++)
            {
                // Getting Cofactor of A[0][f] 
                getCofactor(A, temp, 0, f, n);
                D += sign * A[0][f] * determinant(temp, n - 1);

                // terms are to be added with alternate sign 
                sign = -sign;
            }

            determinant_res = D;

            return determinant_res;
        }

        public double adjoint(double[MAT_MAX_SIZE][MAT_MAX_SIZE] A, double[MAT_MAX_SIZE][MAT_MAX_SIZE] adj)
        {
            if (MATRIX_MAX_SIZE == 1)
            {
                adj[0][0] = 1;
                return;
            }

            // temp is used to store cofactors of A[][] 
            int sign = 1;
            float[MATRIX_MAX_SIZE][MATRIX_MAX_SIZE] temp;

            for (int i = 0; i < MATRIX_MAX_SIZE; i++)
            {
                for (int j = 0; j < MATRIX_MAX_SIZE; j++)
                {
                    // Get cofactor of A[i][j] 
                    getCofactor(A, temp, i, j, MATRIX_MAX_SIZE);

                    // sign of adj[j][i] positive if sum of row 
                    // and column indexes is even. 
                    sign = ((i + j) % 2 == 0) ? 1 : -1;

                    // Interchanging rows and columns to get the 
                    // transpose of the cofactor matrix 
                    adj[j][i] = (sign) * (determinant(temp, MATRIX_MAX_SIZE - 1));
                }
            }
        }

        public bool inverse(double[MAT_MAX_SIZE][MAT_MAX_SIZE] A, double[MAT_MAX_SIZE][MAT_MAX_SIZE] inv, int x)
        {
            // Find determinant of A[][] 
            int det = determinant(A, x);
            if (det == 0)
            {
                return false;
            }

            // Find adjoint 
            double[MATRIX_MAX_SIZE][MATRIX_MAX_SIZE] adj;
            adjoint(A, adj);

            // Find Inverse using formula "inverse(A) = adj(A)/det(A)" 
            for (int i = 0; i < x; i++)
                for (int j = 0; j < x; j++)
                    inv[i][j] = adj[i][j] / (float)(det);

            return true;
        }

        public void transpose(double[MAT_MAX_SIZE][MAT_MAX_SIZE] A, double[MAT_MAX_SIZE][MAT_MAX_SIZE] B, int x)
        { 
        int i, j;
        for (i = 0; i<x; i++) 
        for (j = 0; j<x; j++) 
            *B[i][j] = A[j][i]; 
        
        }

        public void crossproduct (double[][MAT_MAX_SIZE] A, double[][MAT_MAX_SIZE] B, double[][MAT_MAX_SIZE] C, int x, int y)
        {
            int i;
            int j;
            int k;
            int l;
            for (i = 0; i < y; ++i)
            {
                for (j = 0; j < y; j++)
                {
                    C[i][j] = 0;
                    for (k = 0; k < x; k++)
                        *C[i][j] += B[i][k] * A[k][j];
                }
            }
        }

        public void scalarproduct(double[][MAT_MAX_SIZE] A, double[MAT_MAX_SIZE] S, double[MAT_MAX_SIZE] C, int x)
        {
            int i;
            for (i = 0; i < x; i++)
                *C[i] = A[i][0] * S[i];
        }

    }
}
