// TestProgramm.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>

using namespace std;

int main()
{
	setlocale(LC_ALL, "Russian");

	const int lines = 4;
	const int length = 40;
	unsigned char jabberwocky[lines][length] = { "Twas brillig, and the slithy toves",
														"Did gyre and gimble in the wabe:",
														"All mimsy were the borogoves,",
														"And the mome raths outgrabe." };
	unsigned char barmaglot[lines][length] = { "Варкалось. Хливкие шорьки",
													  "Пырялись по наве,",
													  "И хрюкотали зелюки,",
													  "Как мюмзики в мове." };
	
	const int m = 4;
	int matrix[m][m] = { {5, 15, 63, 2},
						 {8, 92, 43, 11},
						 {51, 7, 78, 18},
						 {14, 13, 1, 35}};

	
	int size;
	int* array;

	int a, c, n, sum,
		i, j, k, t,
		left, right, last;

	unsigned char* b;
	unsigned char* p;

	for (i = 0; i < lines; i++)	// 0
		cout << jabberwocky[i] << "\n";	

	for (i = 0; i < lines; i++)
	{
		p = barmaglot[i];
		while (*p) 	//  1
		{
			cout << *p;
			p++;
		}

		cout << "\n";
	}

	i = 0;
	size = 0;
	while (i < lines)
	{
		p = jabberwocky[i];
		while (*p != '\0') // 1
		{
			size++;
			p++;
		}
		i++;
	}

	cout << size << "\n";

	array = new int[size]; // ft 1

	for (i = 0; i < size; i++)//  0
	{		
		n = 0;
		for (j = 0; j < lines; j++) 	// 1
		{
			p = jabberwocky[j];
			a = 0;
			while (*p) 	//  2
			{
				if (n == i) 	//  3
					break;

				n++;
				a++;
				p++;
			}

			if (!(*p)) 	// 2
				continue;

			if (n == i) 	//  2
				break;
		}
	
		c = 0;
		sum = 0;
		b = barmaglot[j];
		while (*b) 	//  1
		{
			if (*b > 192 && *b < 223) 	//  2
				sum -= *b;
			else if (*b > 224 && *b < 255) 	//  3
				sum += *b;
			else if (*b == 192 || *b == 223 ||
					 *b == 224 || *b == 255) 	//  4
				sum += c;

			c++;
			b++;
		}

		array[i] = (*p) * sum / c;
	} 


	left = 1;
	last = right = size - 1;
	do // 0
	{
		for (j = right; j >= left; j--) // 1
			if (array[j - 1] > array[j]) //  2
			{
				int temp = array[j - 1];
				array[j - 1] = array[j];
				array[j] = temp;
				last = j;
			}

		left = last + 1;
		for (j = left; j < right + 1; j++) //  1
			if (array[j - 1] > array[j]) // 2
			{
				int temp = array[j - 1];
				array[j - 1] = array[j];
				array[j] = temp;
				last = j;
			}

		right = last - 1;
	} while (left < right);
	
	for (i = 0; i < size; i++)//  0
		cout << i << "\t" << array[i] << "\n";
	cout << "\n"; //ft 3

	i = 0;
	while (i < size)//  0
	{
		if (i % 2)// 1
		{
			for (k = 0; k < m; k++)	//  2
				if (!(k%2))//  3
				for (t = 0; t < m; t++)	//  4	
					if (!(t % 2))//  5
					{
						n = 0;
						for (j = 0; j < lines; j++) 	//  6
						{
							p = jabberwocky[j];
							a = 0;
							while (*p) 	// 7
							{
								if (n == i) 	//  8
									break;

								n++;
								a++;
								p++;
							}

							if (!(*p)) 	
								continue;

							if (n == i) 	
								break;
						}

						array[i] -= (*p) * matrix[k][t];
					}		
		}
		else // 1
	//	{
			while(true)
			if (!(i % 4)) // 2
			{

				sum = 0;
				for (k = 0; k < m; k++) //  3
				{
					for (t = 0; t < m; t++) // 4
						if (t % 2) //  5
							sum += matrix[k][t];
						else
							sum -= matrix[k][t];
				}

				switch (sum % 5)
				{
				case 0: array[i] /= sum; //  6
					break;
				case 1: //  3
					if (array[i] > 15000) //  7
						array[i] -= sum;
					else
						array[i] += sum;
					break;
				case 2: //  5
					if (array[i] > 10000) //  6
					{
						while (array[i] > 5000) //  7
						{
							switch (array[i] % sum)
							{
								case 0: array[i] /= sum; //  8
									break;
								
								case 0: array[i] /= sum; //  9
									break;
								default:
								{
									if (array[i] > 15000) //  10					
										array[i] -= 15000;									
									else
										array[i] -= 1000;
								} break;
							}
						}
					}
					else
						array[i] += sum;
					break;
				default:
					array[i] /= 10;
					break;
				}
			}
			else
			{
				int index = array[i] % m;
				array[i] *= matrix[index][index];
			}
	//	}

		i++;
	}

	for (i = 0; i < size; i++)//  0
		cout << i << "\t" << array[i] << "\n";
	cout << "\n"; //ft4

	for (i = 0; i < size - 1; i++)//  0
	{
		int min = array[i];
		int index = i;
		for (j = i + 1; j < size; j++)//  1
			if (array[j] > min)//  2
			{
				min = array[j];
				index = j;
			}
		array[index] = array[i];
		array[i] = min;
	}

	for (i = 0; i < size; i++)//  0
	{
		n = 0;
		for (j = 0; j < lines; j++) 	//  1
		{
			p = jabberwocky[j];
			a = 0;
			while (*p) 	//  2
			{
				if (n == i) 	//  3
					break;

				n++;
				a++;
				p++;
			}

			if (!(*p)) 	//  2
				continue;

			if (n == i) 	//  2
				break;
		}

		if ((array[i]>0 && array[i] >= (*p)) || //  1
			(array[i]<0 && -array[i]) >= (*p))
			array[i] /= (*p);
		else
			array[i] += (*p);
	}

	for (i = 0; i < size; i++)//  0
		cout << i << "\t" << array[i] << "\n";
	cout << "\n";//ft5


	system("pause");

    return 0;
}

