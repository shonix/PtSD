import java.util.HashMap;

public class Add extends Binop {
    Expr e1;
    Expr e2;

    public Add(Expr e1, Expr e2) {
        this.e1 = e1;
        this.e2 = e2;
    }

    public String myToString() {
        return e1.toString() + " + " + e2.toString();
    }

    public int eval(HashMap<String, Integer> env) {
        return e1.eval(env) + e2.eval(env);
    }

    public Expr simplify() {
        return new Add(e1.simplify(), e2.simplify());
    }
}
