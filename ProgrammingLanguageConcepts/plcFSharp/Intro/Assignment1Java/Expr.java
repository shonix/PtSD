import java.util.HashMap;

public abstract class Expr {
    public abstract String myToString();
    public abstract int eval(HashMap<String, Integer> env);
    public abstract Expr simplify();
}
