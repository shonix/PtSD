import java.util.HashMap;

public class Sub extends Binop {
    Expr e1;
    Expr e2;

    public Sub (Expr e1, Expr e2) {
        this.e1 = e1;
        this.e2 = e2;
    }

    @Override
    public String toString() {
        return "(" + e1.toString() + " - " + e2.toString() + ")"; 
    }

    @Override
    public int eval(HashMap<String, Integer> env) {
        return e1.eval(env) - e2.eval(env);
    }

    @Override
    public Expr simplify() {
        if (e2.equals(new Csti(0))){
            return e1.simplify();
        }
        if (e1.equals(e2)) {
            return new Csti(0);
        }
        return new Sub(e1.simplify(), e2.simplify());
    }
}
