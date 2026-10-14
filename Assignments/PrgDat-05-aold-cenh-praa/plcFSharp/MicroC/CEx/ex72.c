void arrsum(int n, int arr[], int* sump) {
    int sum;
    int i;

    sum = 0;
    i = 0;

    while (i < n) {
        sum = sum + arr[i];
        i = i + 1;
    }

    *sump = sum;
}

void squares(int n, int arr[]) {
    int i;
    i = 0;

    while (i < n) {
        arr[i] = i * i;
        i = i + 1;
    }
}

void histogram(int n, int ns[], int max, int freq[]) {
    int i;
    int c;

    c = 0;
    while (c <= max) {
        freq[c] = 0;
        c = c + 1;
    }

    i = 0;
    while (i < n) {
        c = ns[i];
        freq[c] = freq[c] + 1;
        i = i + 1;
    }
}

//Exercise 7.2 (i)
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

// Exercise 7.2 (ii)
void testSquares(int n) {
    int arr[20];
    int result;
    result = 0;

    squares(n, arr);
    arrsum(n, arr, &result);

    print result;
    println;
}

// Exercise 7.2 (iii)
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

    c = 0;
    while (c <= 3) {
        print freq[c];
        c = c + 1;
    }
    println;
}

/*
* Array sum: run (fromFile "CEx/ex72.c") [1; 0];;
* Squares: run (fromFile "CEx/ex72.c") [2; 5];;
* Histogram: run (fromFile "CEx/ex72.c") [3; 0];;
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