import java.util.HashMap;

public class Mul extends Binop {
    Expr e1;
    Expr e2;

    public Mul (Expr e1, Expr e2) {
        this.e1 = e1;
        this.e2 = e2;
    }

    @Override
    public String myToString() {
        return e1.toString() + " * " + e2.toString();
    }

    public int eval(HashMap<String, Integer> env){
        return e1.eval(env) * e2.eval(env);
    }

    public Expr simplify() {
        if (e1.equals(new Csti(1))){
            return e2.simplify();
        }
        if (e2.equals(new Csti(1))){
            return e1.simplify();
        }
        if (e1.equals(new Csti(0))){
            return new Csti(0);
        }
        if (e2.equals(new Csti(0))){
            return new Csti(0);
        }
        return new Mul(e1.simplify(), e2.simplify());
    }
}
