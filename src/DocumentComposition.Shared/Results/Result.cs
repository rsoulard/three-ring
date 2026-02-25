using System.Threading.Tasks;

namespace DocumentComposition.Shared.Results;

/// <summary>
/// Represents the outcome of an operation that may succeed or fail.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="Result"/> provides a functional alternative to exceptions for 
/// error handling. It enforces explicit handling of both success and failure 
/// cases, making pipelines more predictable and testable.
/// </para>
/// <para>
/// Common operations include:
/// <list type="bullet">
///   <item><description><see cref="Bind"/> – chains computations that return results.</description></item>
///   <item><description><see cref="Ensure"/> – validates conditions or external processes without altering the value.</description></item>
///   <item><description><see cref="Tap"/> – executes side-effects on success without changing the result.</description></item>
///   <item><description><see cref="Match"/> – projects the result into a final value by handling both cases.</description></item>
/// </list>
/// </para>
/// <para>
/// This type is analogous to <c>Either</c> or <c>Result</c> in functional programming 
/// languages, and is often used in combination with <c>Task</c> to represent 
/// asynchronous operations (<c>Task&lt;Result&lt;T&gt;&gt;</c>).
/// </para>
/// </remarks>
public record Result
{
    public Error Error => error ?? throw new InvalidOperationException("Attempt to access the error value of a successful task.");
    /// <summary>
    /// A collection of errors that occurred during the operation.
    /// </summary>
    public bool IsSuccessful => error is null;

    protected readonly Error? error;

    internal Result()
    {
    }

    internal Result(Error error)
    {
        this.error = error;
    }

    /// <summary>
    /// Create a successful result.
    /// </summary>
    /// <returns>A successful result.</returns>
    public static Result Successful()
    {
        return new();
    }

    /// <summary>
    /// Create a successful result with an object.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    /// <param name="obj">The object.</param>
    /// <returns>A successful result with the object contained in it.</returns>
    public static Result<TObject> From<TObject>(TObject obj)
    {
        return new(obj);
    }

    /// <summary>
    /// Creates a result indicating a failed operation, including the list of errors.
    /// </summary>
    /// <param name="error">The errors to report on the failed result.</param>
    /// <returns>A failed object result.</returns>
    public static Result Failed(Error error)
    {
        var result = new Result(error);

        return result;
    }

    /// <summary>
    /// Creates a result indicating a failed operation, including the list of errors.
    /// </summary>
    /// <param name="errors">The errors to report on the failed result.</param>
    /// <returns>A failed object result.</returns>
    public static Result<TObject> Failed<TObject>(Error error)
    {
        var result = new Result<TObject>(error);

        return result;
    }

    /// <summary>
    /// Chains the current <see cref="Result"/> with another operation that
    /// itself produces a <see cref="Result"/>.
    /// </summary>
    /// <param name="function">
    /// A function to execute if this result is successful. It returns a
    /// new <see cref="Result"/>
    /// </param>
    /// <returns>
    /// The result of <paramref name="function"/> if the current <see cref="Result"/> 
    /// is successful otherwise, propagates the original failure.
    /// </returns>
    /// <remarks>
    /// This method is also known as <c>FlatMap</c> in functional
    /// programming. It allows composition of multiple operations that
    /// may fail without nesting results.
    /// </remarks>
    public Result Bind(Func<Result> function)
    {
        if (IsSuccessful == false)
        {
            return Failed(Error);
        }

        return function();
    }

    /// <summary>
    /// Asynchronously chains the current <see cref="Result"/> with another operation that
    /// itself produces a <see cref="Result"/>.
    /// </summary>
    /// <param name="function">
    /// A function to execute if this result is successful. It returns a
    /// new <see cref="Result"/>
    /// </param>
    /// <returns>
    /// A task wrapped around the result of <paramref name="function"/> if the current <see cref="Result"/>
    /// is successful otherwise, propagates the original failure.
    /// </returns>
    /// <remarks>
    /// This method is also known as <c>FlatMap</c> in functional
    /// programming. It allows composition of multiple operations that
    /// may fail without nesting results.
    /// </remarks>
    public async Task<Result> BindAsync(Func<Task<Result>> function)
    {
        if (IsSuccessful == false)
        {
            return Failed(Error);
        }

        return await function();
    }

    /// <summary>
    /// Executes the specified action if the current <see cref="Result"/> is successful
    /// without altering the success state of the <see cref="Result"/>.
    /// </summary>
    /// <param name="action">
    /// An action to perform when the current <see cref="Result"/> is successful. The
    /// action is intended for side-effects such as logging, metrics, or notifications.
    /// </param>
    /// <returns>
    /// The original <see cref="Result"/>,  unchanged, regardless of whether the action was executed.
    /// </returns>
    /// <remarks>
    /// <para>
    /// <c>Tap</c> is useful for observing or reacting to successful results without
    /// modifying the pipeline. It is analogous to <c>Peek</c> in LINQ.
    /// </para>
    /// </remarks>
    public Result Tap(Action action)
    {
        if (IsSuccessful)
        {
            action();
        }

        return this;
    }

    /// <summary>
    /// Asynchronously executes the specified action if the current <see cref="Result"/> is successful
    /// without altering the success state of the <see cref="Result"/>.
    /// </summary>
    /// <param name="action">
    /// An action to perform when the current <see cref="Result"/> is successful. The
    /// action is intended for side-effects such as logging, metrics, or notifications.
    /// </param>
    /// <returns>
    /// A task wrapped around the original <see cref="Result"/>,  unchanged, regardless of whether the action was executed.
    /// </returns>
    /// <remarks>
    /// <para>
    /// <c>Tap</c> is useful for observing or reacting to successful results without
    /// modifying the pipeline. It is analogous to <c>Peek</c> in LINQ.
    /// </para>
    /// </remarks>
    public async Task<Result> TapAsync(Func<Task> action)
    {
        if (IsSuccessful)
        {
            await action();
        }

        return this;
    }

    /// <summary>
    /// Calls one of the two functions depending on whether the current <see cref="Result"/>
    /// was successful or was a failure, producing a final projected value.
    /// </summary>
    /// <typeparam name="U">The type of the value returned by the projection functions.</typeparam>
    /// <param name="onSuccess">
    /// A function to call if the current <see cref="Result"/> is successful.
    /// It returns a value of type <typeparamref name="U"/>.
    /// </param>
    /// <param name="onFailure">
    /// A function to call if the current <see cref="Result"/> is not successful.
    /// It returns a value of type <typeparamref name="U"/>.
    /// </param>
    /// <returns>
    /// The value produced by either <paramref name="onSuccess"/> or <paramref name="onFailure"/> depending
    /// on the state of the current <see cref="Result"/>.
    /// </returns>
    /// <remarks>
    /// <c>Match</c> is a way of "unwrapping" a <see cref="Result"/>. It ensures both success and failure
    /// cases are handled explicitly, avoiding null checks or exception handling.
    /// </remarks>
    public U Match<U>(Func<U> onSuccess, Func<Error, U> onFailure)
    {
        return IsSuccessful ? onSuccess() : onFailure(Error);
    }

    /// <summary>
    /// Asynchronously calls one of the two functions depending on whether the current <see cref="Result"/>
    /// was successful or was a failure, producing a final projected value.
    /// </summary>
    /// <typeparam name="U">The type of the value returned by the projection functions.</typeparam>
    /// <param name="onSuccess">
    /// A function to call if the current <see cref="Result"/> is successful.
    /// It returns a value of type <typeparamref name="U"/>.
    /// </param>
    /// <param name="onFailure">
    /// A function to call if the current <see cref="Result"/> is not successful.
    /// It returns a value of type <typeparamref name="U"/>.
    /// </param>
    /// <returns>
    /// A task wrapped around the value produced by either <paramref name="onSuccess"/> or <paramref name="onFailure"/> depending
    /// on the state of the current <see cref="Result"/>.
    /// </returns>
    /// <c>Match</c> is a way of "unwrapping" a <see cref="Result"/>. It ensures both success and failure
    /// cases are handled explicitly, avoiding null checks or exception handling.
    /// </remarks>
    public async Task<U> MatchAsync<U>(Func<Task<U>> onSuccess, Func<Error, Task<U>> onFailure)
    {
        return IsSuccessful ? await onSuccess() : await onFailure(Error);
    }

    /// <summary>
    /// Validates the current <see cref="Result"/> by executing a condition or process that itself produces a <see cref="Result"/>. 
    /// </summary>
    /// <param name="process">
    /// A function to call if the current <see cref="Result"/> is successful. It should return a plain
    /// <see cref="Result"/> indicating whether the validation or external process succeeded. The
    /// current <see cref="Result"/> is not mutated.
    /// </param>
    /// <returns>
    /// The original <see cref="Result"/> if both it and <paramref name="process"/> are successful
    /// otherwise, propagates the failure from whichever step failed.
    /// </returns>
    /// <remarks>
    /// <para>
    /// <c>Ensure</c> is intended for guard checks or external validations that must succeed
    /// before continuing the pipeline. It does not alter the contained value, only short‑circuits
    /// on failure.
    /// </para>
    /// <para>
    /// This method is useful for scenarios such as database commits, external service calls,
    /// or business rule validations where success/failure must be enforced without changing
    /// the payload.
    /// </para>
    /// </remarks>
    public Result Ensure(Func<Result> process)
    {
        if (IsSuccessful)
        {
            var result = process();
            if (result.IsSuccessful == false)
            {
                return Failed(result.Error);
            }
        }

        return this;
    }

    /// <summary>
    /// Validates the current <see cref="Result"/> by asynchronously executing a condition or process that itself produces a <see cref="Result"/>. 
    /// </summary>
    /// <param name="process">
    /// A function to call if the current <see cref="Result"/> is successful. It should return a plain
    /// <see cref="Result"/> indicating whether the validation or external process succeeded. The
    /// current <see cref="Result"/> is not mutated.
    /// </param>
    /// <returns>
    /// A task that is wrapped around the original <see cref="Result"/> if both it and <paramref name="process"/> are successful
    /// otherwise, propagates the failure from whichever step failed.
    /// </returns>
    /// <remarks>
    /// <para>
    /// <c>Ensure</c> is intended for guard checks or external validations that must succeed
    /// before continuing the pipeline. It does not alter the contained value, only short‑circuits
    /// on failure.
    /// </para>
    /// <para>
    /// This method is useful for scenarios such as database commits, external service calls,
    /// or business rule validations where success/failure must be enforced without changing
    /// the payload.
    /// </para>
    /// </remarks>
    public async Task<Result> EnsureAsync(Func<Task<Result>> function)
    {
        if (IsSuccessful)
        {
            var result = await function();
            if (result.IsSuccessful == false)
            {
                return Failed(result.Error);
            }
        }

        return this;
    }
}

/// <summary>
/// Represents the outcome of an operation that may succeed or fail, 
/// encapsulating either a value of type <typeparamref name="T"/> on success 
/// or an error message on failure.
/// </summary>
/// <typeparam name="T">
/// The type of the value contained in the result when the operation succeeds.
/// </typeparam>
/// <remarks>
/// <para>
/// <see cref="Result{T}"/> provides a functional alternative to exceptions for 
/// error handling. It enforces explicit handling of both success and failure 
/// cases, making pipelines more predictable and testable.
/// </para>
/// <para>
/// Common operations include:
/// <list type="bullet">
///   <item><description><see cref="Bind"/> – chains computations that return results.</description></item>
///   <item><description><see cref="Map"/> – transforms the contained value while preserving success/failure.</description></item>
///   <item><description><see cref="Ensure"/> – validates conditions or external processes without altering the value.</description></item>
///   <item><description><see cref="Tap"/> – executes side-effects on success without changing the result.</description></item>
///   <item><description><see cref="Match"/> – projects the result into a final value by handling both cases.</description></item>
/// </list>
/// </para>
/// <para>
/// This type is analogous to <c>Either</c> or <c>Result</c> in functional programming 
/// languages, and is often used in combination with <c>Task</c> to represent 
/// asynchronous operations (<c>Task&lt;Result&lt;T&gt;&gt;</c>).
/// </para>
/// </remarks>
public record Result<T> : Result
{
    private readonly T? value;
    public T Value => !IsSuccessful ? throw new Exception("Attempted to unwrap a failed result for a value.") : value!;

    internal Result(Error error) : base(error)
    {

    }

    internal Result(T value)
    {
        this.value = value;
    }

    internal Result()
    {
        value = default;
    }

    /// <summary>
    /// Chains the current <see cref="Result{T}"/> with another operation that
    /// itself produces a <see cref="Result{U}"/>.
    /// </summary>
    /// <typeparam name="U">The type of the value contained in the result returned by <paramref name="function"/>.</typeparam>
    /// <param name="function">
    /// A function to execute if this result is successful. It receives the 
    /// inner value or the current <see cref="Result{T}"/> and returns a new <see cref="Result{U}"/>
    /// </param>
    /// <returns>
    /// The result of <paramref name="function"/> if the current <see cref="Result{T}"/> 
    /// is successful otherwise, propagates the original failure.
    /// </returns>
    /// <remarks>
    /// This method is also known as <c>FlatMap</c> in functional
    /// programming. It allows composition of multiple operations that
    /// may fail without nesting results.
    /// </remarks>
    public Result<U> Bind<U>(Func<T, Result<U>> function)
    {
        if (IsSuccessful == false)
        {
            return Failed<U>(Error);
        }

        return function(Value);
    }

    /// <summary>
    /// Asynchronously chains the current <see cref="Result{T}"/> with another operation that
    /// itself produces a <see cref="Result{U}"/>.
    /// </summary>
    /// <typeparam name="U">The type of the value contained in the result returned by <paramref name="function"/>.</typeparam>
    /// <param name="function">
    /// A function to execute if this result is successful. It receives the 
    /// inner value or the current <see cref="Result{T}"/> and returns a new <see cref="Result{U}"/>
    /// </param>
    /// <returns>
    /// A task wrapped around the result of <paramref name="function"/> if the current <see cref="Result{T}"/> 
    /// is successful otherwise, propagates the original failure.
    /// </returns>
    /// <remarks>
    /// This method is also known as <c>FlatMap</c> in functional
    /// programming. It allows composition of multiple operations that
    /// may fail without nesting results.
    /// </remarks>
    public async Task<Result<U>> BindAsync<U>(Func<T, Task<Result<U>>> function)
    {
        if (IsSuccessful == false)
        {
            return Failed<U>(Error);
        }

        return await function(Value);
    }

    /// <summary>
    /// Chains the current <see cref="Result{T}"/> with another operation that
    /// itself produces a plain <see cref="Result"/>.
    /// </summary>
    /// <param name="function">
    /// A function to execute if this result is successful. It receives the 
    /// inner value or the current <see cref="Result{T}"/> and returns a new <see cref="Result"/>
    /// </param>
    /// <returns>
    /// The result of <paramref name="function"/> if the current <see cref="Result"/> 
    /// is successful otherwise, propagates the original failure.
    /// </returns>
    /// <remarks>
    /// This method is also known as <c>FlatMap</c> in functional
    /// programming. It allows composition of multiple operations that
    /// may fail without nesting results.
    /// </remarks>
    public Result Bind(Func<T, Result> function)
    {
        if (IsSuccessful == false)
        {
            return Failed(Error);
        }

        return function(Value);
    }

    /// <summary>
    /// Asynchronously chains the current <see cref="Result{T}"/> with another operation that
    /// itself produces a plain <see cref="Result"/>.
    /// </summary>
    /// <param name="function">
    /// A function to execute if this result is successful. It receives the 
    /// inner value or the current <see cref="Result{T}"/> and returns a new <see cref="Result"/>
    /// </param>
    /// <returns>
    /// A task that is wrapped around the result of <paramref name="function"/> if the current <see cref="Result"/> 
    /// is successful otherwise, propagates the original failure.
    /// </returns>
    /// <remarks>
    /// This method is also known as <c>FlatMap</c> in functional
    /// programming. It allows composition of multiple operations that
    /// may fail without nesting results.
    /// </remarks>
    public async Task<Result> BindAsync(Func<T, Task<Result>> function)
    {
        if (IsSuccessful == false)
        {
            return Failed(Error);
        }

        return await function(Value);
    }

    /// <summary>
    /// Transforms the inner value in the current <see cref="Result{T}"/>, if it is successful,
    /// into a new value producing a <see cref="Result{U}"/> while preserving the success or 
    /// failure state.
    /// </summary>
    /// <typeparam name="U">The type of the value returned by the <paramref name="mapper"/>.</typeparam>
    /// <param name="mapper">
    /// A function to apply to the inner value if the current <see cref="Result{T}"/> is successful.
    /// It receives the current inner value and returns a new value of type <typeparamref name="U"/>
    /// </param>
    /// <returns>
    /// A new <see cref="Result{U}"/> containing the transformed value if the original <see cref="Result{T}"/>
    /// is successful; otherwise, a failure result with the original error.
    /// </returns>
    /// <remarks>
    /// <para>
    /// <c>Map</c> is intended for pure transformations of the contained value. It does not
    /// alter the success or failure state and does not allow the transformation function to
    /// return another <see cref="Result{U}"/>. For that scenario, use <see cref="Bind"/>.
    /// </para>
    /// <para>
    /// This method is analogous to <c>Select</c> in LINQ or <c>fmap</c> in functional programming.
    /// </para>
    /// </remarks>
    public Result<U> Map<U>(Func<T, U> mapper)
    {
        if (IsSuccessful == false)
        {
            return Failed<U>(Error);
        }

        return From(mapper(Value));
    }

    /// <summary>
    /// Asynchronously transforms the inner value in the current <see cref="Result{T}"/>, if it is successful,
    /// into a new value producing a <see cref="Result{U}"/> while preserving the success or 
    /// failure state.
    /// </summary>
    /// <typeparam name="U">The type of the value returned by the <paramref name="mapper"/>.</typeparam>
    /// <param name="mapper">
    /// A function to apply to the inner value if the current <see cref="Result{T}"/> is successful.
    /// It receives the current inner value and returns a new value of type <typeparamref name="U"/>
    /// </param>
    /// <returns>
    /// A task wrapped around a new <see cref="Result{U}"/> containing the transformed value if the original <see cref="Result{T}"/>
    /// is successful; otherwise, a failure result with the original error.
    /// </returns>
    /// <remarks>
    /// <para>
    /// <c>Map</c> is intended for pure transformations of the contained value. It does not
    /// alter the success or failure state and does not allow the transformation function to
    /// return another <see cref="Result{U}"/>. For that scenario, use <see cref="BindAsync"/>.
    /// </para>
    /// <para>
    /// This method is analogous to <c>Select</c> in LINQ or <c>fmap</c> in functional programming.
    /// </para>
    /// </remarks>
    public async Task<Result<U>> MapAsync<U>(Func<T, Task<U>> mapper)
    {
        if (IsSuccessful == false)
        {
            return Failed<U>(Error);
        }

        var newValue = await mapper(Value);
        return From(newValue);
    }

    /// <summary>
    /// Executes the specified action if the current <see cref="Result{T}"/> is successful
    /// without altering the contained value or the success state of the <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="action">
    /// An action to perform when the current <see cref="Result"/> is successful. The
    /// action is intended for side-effects such as logging, metrics, or notifications,
    /// and must not mutate the contained value. Note that there is no way of preventing
    /// this in C#.
    /// </param>
    /// <returns>
    /// The original <see cref="Result"/>,  unchanged, regardless of whether the action was executed.
    /// </returns>
    /// <remarks>
    /// <para>
    /// <c>Tap</c> is useful for observing or reacting to successful results without
    /// modifying the pipeline. It is analogous to <c>Peek</c> in LINQ.
    /// </para>
    /// </remarks>
    public Result<T> Tap(Action<T> action)
    {
        if (IsSuccessful)
        {
            action(Value);
        }

        return this;
    }

    /// <summary>
    /// Asynchronously executes the specified action if the current <see cref="Result{T}"/> is successful
    /// without altering the contained value or the success state of the <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="action">
    /// An action to perform when the current <see cref="Result"/> is successful. The
    /// action is intended for side-effects such as logging, metrics, or notifications,
    /// and must not mutate the contained value. Note that there is no way of preventing
    /// this in C#.
    /// </param>
    /// <returns>
    /// A task wrapped around the original <see cref="Result"/>,  unchanged, regardless of whether the action was executed.
    /// </returns>
    /// <remarks>
    /// <para>
    /// <c>Tap</c> is useful for observing or reacting to successful results without
    /// modifying the pipeline. It is analogous to <c>Peek</c> in LINQ.
    /// </para>
    /// </remarks>
    public async Task<Result<T>> TapAsync(Func<T, Task> action)
    {
        if (IsSuccessful)
        {
            await action(Value);
        }

        return this;
    }

    /// <summary>
    /// Calls one of the two functions depending on whether the current <see cref="Result{T}"/>
    /// was successful or was a failure, producing a final projected value.
    /// </summary>
    /// <typeparam name="U">The type of the value returned by the projection functions.</typeparam>
    /// <param name="onSuccess">
    /// A function to call if the current <see cref="Result"/> is successful.
    /// It receives the inner value of the current <see cref="Result"/> and returns
    /// a value of type <typeparamref name="U"/>.
    /// </param>
    /// <param name="onFailure">
    /// A function to call if the current <see cref="Result"/> is not successful.
    /// It receives the error from the current <see cref="Result"/> and returns a value of type <typeparamref name="U"/>.
    /// </param>
    /// <returns>
    /// The value produced by either <paramref name="onSuccess"/> or <paramref name="onFailure"/> depending
    /// on the state of the current <see cref="Result"/>.
    /// </returns>
    /// <remarks>
    /// <c>Match</c> is a way of "unwrapping" a <see cref="Result"/>. It ensures both success and failure
    /// cases are handled explicitly, avoiding null checks or exception handling.
    /// </remarks>
    public U Match<U>(Func<T, U> onSuccess, Func<Error, U> onFailure)
    {
        return IsSuccessful == true ? onSuccess(Value) : onFailure(Error);
    }

    /// <summary>
    /// Asynchronously calls one of the two functions depending on whether the current <see cref="Result{T}"/>
    /// was successful or was a failure, producing a final projected value.
    /// </summary>
    /// <typeparam name="U">The type of the value returned by the projection functions.</typeparam>
    /// <param name="onSuccess">
    /// A function to call if the current <see cref="Result"/> is successful.
    /// It receives the inner value of the current <see cref="Result"/> and returns
    /// a value of type <typeparamref name="U"/>.
    /// </param>
    /// <param name="onFailure">
    /// A function to call if the current <see cref="Result"/> is not successful.
    /// It receives the error from the current <see cref="Result"/> and returns a value of type <typeparamref name="U"/>.
    /// </param>
    /// <returns>
    /// A task wrapped around the value produced by either <paramref name="onSuccess"/> or <paramref name="onFailure"/> depending
    /// on the state of the current <see cref="Result"/>.
    /// </returns>
    /// <remarks>
    /// <c>Match</c> is a way of "unwrapping" a <see cref="Result"/>. It ensures both success and failure
    /// cases are handled explicitly, avoiding null checks or exception handling.
    /// </remarks>
    public async Task<U> MatchAsync<U>(Func<T, Task<U>> onSuccess, Func<Error, Task<U>> onFailure)
    {
        return IsSuccessful ? await onSuccess(Value) : await onFailure(Error);
    }

    /// <summary>
    /// Validates the current <see cref="Result{T}"/> by executing a condition or process that itself produces a <see cref="Result"/>. 
    /// </summary>
    /// <param name="process">
    /// A function to call if the current <see cref="Result{T}"/> is successful. It should return a plain
    /// <see cref="Result"/> indicating whether the validation or external process succeeded. The
    /// current <see cref="Result{T}"/> or its inner value is not mutated.
    /// </param>
    /// <returns>
    /// The original <see cref="Result{T}"/> if both it and <paramref name="process"/> are successful
    /// otherwise, propagates the failure from whichever step failed.
    /// </returns>
    /// <remarks>
    /// <para>
    /// <c>Ensure</c> is intended for guard checks or external validations that must succeed
    /// before continuing the pipeline. It does not alter the contained value, only short‑circuits
    /// on failure.
    /// </para>
    /// <para>
    /// This method is useful for scenarios such as database commits, external service calls,
    /// or business rule validations where success/failure must be enforced without changing
    /// the payload.
    /// </para>
    /// </remarks>
    public new Result<T> Ensure(Func<Result> process)
    {
        if (IsSuccessful)
        {
            var result = process();
            if (result.IsSuccessful == false)
            {
                return Failed<T>(result.Error);
            }
        }

        return this;
    }

        /// <summary>
    /// Validates the current <see cref="Result{T}"/> by executing a condition or process that itself produces a <see cref="Result"/>. 
    /// </summary>
    /// <param name="process">
    /// A function to call if the current <see cref="Result{T}"/> is successful. It should return a plain
    /// <see cref="Result"/> indicating whether the validation or external process succeeded. The
    /// current <see cref="Result{T}"/> or its inner value is not mutated.
    /// </param>
    /// <returns>
    /// The original <see cref="Result{T}"/> if both it and <paramref name="process"/> are successful
    /// otherwise, propagates the failure from whichever step failed.
    /// </returns>
    /// <remarks>
    /// <para>
    /// <c>Ensure</c> is intended for guard checks or external validations that must succeed
    /// before continuing the pipeline. It does not alter the contained value, only short‑circuits
    /// on failure.
    /// </para>
    /// <para>
    /// This method is useful for scenarios such as database commits, external service calls,
    /// or business rule validations where success/failure must be enforced without changing
    /// the payload.
    /// </para>
    /// </remarks>
    public Result<T> Ensure(Func<T, Result> process)
    {
        if (IsSuccessful)
        {
            var result = process(Value);
            if (result.IsSuccessful == false)
            {
                return Failed<T>(result.Error);
            }
        }

        return this;
    }

    /// <summary>
    /// Validates the current <see cref="Result{T}"/> by asynchronously executing a condition or process that itself produces a <see cref="Result"/>. 
    /// </summary>
    /// <param name="process">
    /// A function to call if the current <see cref="Result{T}"/> is successful. It should return a plain
    /// <see cref="Result"/> indicating whether the validation or external process succeeded. The
    /// current <see cref="Result{T}"/> or its inner value is not mutated.
    /// </param>
    /// <returns>
    /// A task wrapped around the original <see cref="Result{T}"/> if both it and <paramref name="process"/> are successful
    /// otherwise, propagates the failure from whichever step failed.
    /// </returns>
    /// <remarks>
    /// <para>
    /// <c>Ensure</c> is intended for guard checks or external validations that must succeed
    /// before continuing the pipeline. It does not alter the contained value, only short‑circuits
    /// on failure.
    /// </para>
    /// <para>
    /// This method is useful for scenarios such as database commits, external service calls,
    /// or business rule validations where success/failure must be enforced without changing
    /// the payload.
    /// </para>
    /// </remarks>
    public new async Task<Result<T>> EnsureAsync(Func<Task<Result>> function)
    {
        if (IsSuccessful)
        {
            var result = await function();
            if (result.IsSuccessful == false)
            {
                return Failed<T>(result.Error);
            }
        }

        return this;
    }

    /// <summary>
    /// Validates the current <see cref="Result{T}"/> by asynchronously executing a condition or process that itself produces a <see cref="Result"/>. 
    /// </summary>
    /// <param name="process">
    /// A function to call if the current <see cref="Result{T}"/> is successful. It should return a plain
    /// <see cref="Result"/> indicating whether the validation or external process succeeded. The
    /// current <see cref="Result{T}"/> or its inner value is not mutated.
    /// </param>
    /// <returns>
    /// A task wrapped around the original <see cref="Result{T}"/> if both it and <paramref name="process"/> are successful
    /// otherwise, propagates the failure from whichever step failed.
    /// </returns>
    /// <remarks>
    /// <para>
    /// <c>Ensure</c> is intended for guard checks or external validations that must succeed
    /// before continuing the pipeline. It does not alter the contained value, only short‑circuits
    /// on failure.
    /// </para>
    /// <para>
    /// This method is useful for scenarios such as database commits, external service calls,
    /// or business rule validations where success/failure must be enforced without changing
    /// the payload.
    /// </para>
    /// </remarks>
    public async Task<Result<T>> EnsureAsync(Func<T, Task<Result>> function)
    {
        if (IsSuccessful)
        {
            var result = await function(Value);
            if (result.IsSuccessful == false)
            {
                return Failed<T>(result.Error);
            }
        }

        return this;
    }

    /// <summary>
    /// Combine two independent <see cref="Result{T}"/> values into a single <see cref="Result{(T, U)}"/> if
    /// both <see cref="Result{T}"/> are successful.
    /// </summary>
    /// <typeparam name="U">The type of the value held by the other <see cref="Result{T}"/></typeparam>
    /// <param name="otherResult">
    /// The <see cref="Result{U}"/> to combine with the current <see cref="Result{T}"/>.
    /// </param>
    /// <returns>
    /// A new <see cref="Result{(T, U)}"/> that represents either combined success or the individual
    /// failure of one of the component <see cref="Result{T}"/>.
    /// </returns>
    /// <remarks>
    /// <c>Combine</c> is useful for merging two independent <see cref="Result{T}"/> into a single one.
    /// It is analogous to <c>Zip</c> in LINQ or <c>liftA2</c> in functional programming.
    /// </remarks>
    public Result<(T, U)> Combine<U>(Result<U> otherResult)
    {
        if (IsSuccessful == false)
        {
            return Failed<(T, U)>(Error);
        }

        if (otherResult.IsSuccessful == false)
        {
            return Failed<(T, U)>(otherResult.Error);
        }

        return From((Value, otherResult.Value));
    }

    /// <summary>
    /// Asynchronously combine two independent <see cref="Result{T}"/> values into a single <see cref="Result{(T, U)}"/> if
    /// both <see cref="Result{T}"/> are successful.
    /// </summary>
    /// <typeparam name="U">The type of the value held by the other <see cref="Result{T}"/></typeparam>
    /// <param name="otherResult">
    /// The <see cref="Result{U}"/> to combine with the current <see cref="Result{T}"/>.
    /// </param>
    /// <returns>
    /// A new <see cref="Result{(T, U)}"/> that represents either combined success or the individual
    /// failure of one of the component <see cref="Result{T}"/>.
    /// </returns>
    /// <remarks>
    /// <c>Combine</c> is useful for merging two independent <see cref="Result{T}"/> into a single one.
    /// It is analogous to <c>Zip</c> in LINQ or <c>liftA2</c> in functional programming.
    /// </remarks>
    public async Task<Result<(T, U)>> CombineAsync<U>(Task<Result<U>> otherResult)
    {
        if (IsSuccessful == false)
        {
            return Failed<(T, U)>(Error);
        }

        var result = await otherResult;
        if (result.IsSuccessful == false)
        {
            return Failed<(T, U)>(result.Error);
        }

        return From((Value, result.Value));
    }

    /// <summary>
    /// Applies a mutation to the value of this <see cref="Result{T}"/> if it is successful.
    /// Otherwise, return the failed result. 
    /// </summary>
    /// <param name="mutator">An action that mutates the contained value when the result is successful.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> instance with the mutated value, allowing further chaining.
    /// </returns>
    /// <remarks>
    /// <c>Apply</c> is intended for in-place updates on mutable objects. If the result is failed, the mutation
    /// is skipped and the original failure is propagated unchanged. This operator is useful when you want to
    /// modify the underlying value while preserving the result pipeline.
    /// </remarks>
    public Result<T> Apply(Action<T> mutator)
    {
        if (IsSuccessful)
        {
            mutator(Value);
        }

        return this;
    }

    /// <summary>
    /// Asynchronously applies a mutation to the value of this <see cref="Result{T}"/> if it is successful.
    /// Otherwise, return the failed result. 
    /// </summary>
    /// <param name="asyncMutator">An asynchronous action that mutates the contained value when the result is successful.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> instance with the mutated value, allowing further chaining.
    /// </returns>
    /// <remarks>
    /// <c>Apply</c> is intended for in-place updates on mutable objects. If the result is failed, the mutation
    /// is skipped and the original failure is propagated unchanged. This operator is useful when you want to
    /// modify the underlying value while preserving the result pipeline.
    /// </remarks>
    public async Task<Result<T>> ApplyAsync(Func<T, Task> asyncMutator)
    {
        if (IsSuccessful)
        {
            await asyncMutator(Value);
        }

        return this;
    }
}
