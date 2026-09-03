
//Contains par 1.4
public class Main {
    public static void main(String[] args) {

        //Exercise 1.4 (ii)
        Expr expT1 = new Sub (new Csti(15), new Mul(new Csti(2), new Csti(5)));
        //should be (15 - (2 * 5))
        Expr expT2 = new Add (new Csti(5), new Csti(6));
        //should be (5 + 6)
        Expr expT3 = new Mul (new Add(new Csti(5), new Csti(3)), new Sub(new Csti(5), new Csti(3)));
        //should be ((5 + 3) * (5 - 3))

        System.out.println(expT1.myToString());
        System.out.println(expT2.myToString());
        System.out.println(expT3.myToString());
    }
}