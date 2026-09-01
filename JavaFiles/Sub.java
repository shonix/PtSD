import java.util.HashMap;

public class Sub extends Binop {
    Expr e1;
    Expr e2;

    public Sub (Expr e1, Expr e2) {
        this.e1 = e1;
        this.e2 = e2;
    }

    public String myToString() {
        return e1.toString() + " - " + e2.toString();
    }

    public int eval(HashMap<String, Integer> env) {
        return e1.eval(env) - e2.eval(env);
    }
}
