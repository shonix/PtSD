import java.util.HashMap;

public class Var extends Expr {
    String s;

    public Var (String x) {
        this.s = x;
    }

    @Override
    public String toString() {
        return s;
    }

    public int eval(HashMap<String, Integer> env){
        return env.get(s);
    }

    public Expr simplify() {
        return this;
    }
}
