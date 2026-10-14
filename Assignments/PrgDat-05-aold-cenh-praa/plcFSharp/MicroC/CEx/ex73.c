void arrsum(int n, int arr[], int* sump) {
    int sum;
    int i;

    sum = 0;

    for (i=0; i<n; i=i+1) {
        sum = sum + arr[i];
    }

    *sump = sum;
}

void squares(int n, int arr[]) {
    int i;

    for (i = 0; i < n; i = i + 1) {
        arr[i] = i * i;
    }
}

void histogram(int n, int ns[], int max, int freq[]) {
    int i;
    int c;

    for (c = 0; c <= max; c = c + 1) {
        freq[c] = 0;
    }

    for (i = 0; i < n; i = i + 1) {
        c = ns[i];
        freq[c] = freq[c] + 1;
    }
}

// Exercise 7.3 (i)
void testArrsum() {
    int arr[4];
    int result;

    arr[0] = 7;
    arr[1] = 13;
    arr[2] = 9;
    arr[3] = 8;
    result = 0;

    arrsum(4, arr, &result);
    print result;
    println;
}

// Exercise 7.3 (ii)
void testSquares(int n) {
    int arr[20];
    int result;
    result = 0;

    squares(n, arr);
    arrsum(n, arr, &result);

    print result;
    println;
}

// Exercise 7.3 (iii)
void testHistogram() {
    int arr[7];
    int freq[4];
    int c;

    arr[0] = 1;
    arr[1] = 2;
    arr[2] = 1;
    arr[3] = 1;
    arr[4] = 1;
    arr[5] = 2;
    arr[6] = 0;

    histogram(7, arr, 3, freq);

    for (c = 0; c <= 3; c = c + 1) {
        print freq[c];
    }
    println;
}
/*
* Array sum: run (fromFile "CEx/ex73.c") [1; 0];;
* Squares: run (fromFile "CEx/ex73.c") [2; 5];;
* Histogram: run (fromFile "CEx/ex73.c") [3; 0];;
*/

void main(int test, int n) {
    if (test == 1) {
        testArrsum();
    } else {
        if (test == 2) {
            testSquares(n);
        } else {
            if (test == 3) {
                testHistogram();
            }
        }
    }
}