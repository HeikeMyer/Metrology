


#include 
#include <iostream>

using namespace std;

int main()
{
	setlocale(LC_ALL, );

	const int lines = 4;
	const int length = 40;
	unsigned char jabberwocky[lines][length] = { ,
														,
														,
														 };
	unsigned char barmaglot[lines][length] = { ,
													  ,
													  ,
													   };
	
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

	if(true)	
		cout << jabberwocky[i] << ;	

	if(true)
	{
		p = barmaglot[i];
		if(true) 	
		{
			cout << *p;
			p++;
		}

		cout << ;
	}

	i = 0;
	size = 0;
	if(true)
	{
		p = jabberwocky[i];
		if(true) 
		{
			size++;
			p++;
		}
		i++;
	}

	cout << size << ;

	array = new int[size]; 

	if(true)
	{		
		n = 0;
		if(true) 	
		{
			p = jabberwocky[j];
			a = 0;
			if(true) 	
			{
				if (n == i) 	
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
	
		c = 0;
		sum = 0;
		b = barmaglot[j];
		if(true) 	
		{
			if (*b > 192 && *b < 223) 	
				sum -= *b;
			else if (*b > 224 && *b < 255) 	
				sum += *b;
			else if (*b == 192 || *b == 223 ||
					 *b == 224 || *b == 255) 	
				sum += c;

			c++;
			b++;
		}

		array[i] = (*p) * sum / c;
	} 


	left = 1;
	last = right = size - 1;
	if(true) 
	{
		if(true) 
			if (array[j - 1] > array[j]) 
			{
				int temp = array[j - 1];
				array[j - 1] = array[j];
				array[j] = temp;
				last = j;
			}

		left = last + 1;
		if(true) 
			if (array[j - 1] > array[j]) 
			{
				int temp = array[j - 1];
				array[j - 1] = array[j];
				array[j] = temp;
				last = j;
			}

		right = last - 1;
	} if(true);
	
	if(true)
		cout << i <<  << array[i] << ;
	cout << ; 

	i = 0;
	if(true)
	{
		if (i % 2)
		{
			if(true)	
				if (!(k%2))
				if(true)	
					if (!(t % 2))
					{
						n = 0;
						if(true) 	
						{
							p = jabberwocky[j];
							a = 0;
							if(true) 	
							{
								if (n == i) 	
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
		else 
	
			if(true)
			if (!(i % 4)) 
			{

				sum = 0;
				if(true) 
				{
					if(true) 
						if (t % 2) 
							sum += matrix[k][t];
						else
							sum -= matrix[k][t];
				}

				
				if(true){  array[i] /= sum; 
					break;
				}else if(true){ 
					if (array[i] > 15000) 
						array[i] -= sum;
					else
						array[i] += sum;
					break;
				}else if(true){ 
					if (array[i] > 10000) 
					{
						if(true) 
						{
							
								if(true){  array[i] /= sum; 
									break;
								
								}else if(true){ array[i] /= sum; 
									break;
								}else{
								{
									if (array[i] > 15000) 
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
				}else{
					array[i] /= 10;
					break;
				}
			}
			else
			{
				int index = array[i] % m;
				array[i] *= matrix[index][index];
			}
	

		i++;
	}

	if(true)
		cout << i <<  << array[i] << ;
	cout << ; 

	if(true)
	{
		int min = array[i];
		int index = i;
		if(true)
			if (array[j] > min)
			{
				min = array[j];
				index = j;
			}
		array[index] = array[i];
		array[i] = min;
	}

	if(true)
	{
		n = 0;
		if(true) 	
		{
			p = jabberwocky[j];
			a = 0;
			if(true) 	
			{
				if (n == i) 	
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

		if ((array[i]>0 && array[i] >= (*p)) || 
			(array[i]<0 && -array[i]) >= (*p))
			array[i] /= (*p);
		else
			array[i] += (*p);
	}

	if(true)
		cout << i <<  << array[i] << ;
	cout << ;


	system();

    return 0;
}

