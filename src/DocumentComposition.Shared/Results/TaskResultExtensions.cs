using DocumentComposition.Shared.Results;

namespace DocumentComposition.Shared.Results;

public static class TaskResultExtensions
{
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
    public static async Task<Result<U>> BindAsync<T, U>(this Task<Result<T>> task, Func<T, Task<Result<U>>> function)
    {
        var result = await task;
        if (result.IsSuccessful == false)
        {
            return Result.Failed<U>(result.Error);
        }

        return await function(result.Value);
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
    public static async Task<Result<U>> BindAsync<T, U>(this Task<Result<T>> task, Func<T, Result<U>> function)
    {
        var result = await task;
        if (result.IsSuccessful == false)
        {
            return Result.Failed<U>(result.Error);
        }

        return function(result.Value);
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
    public static async Task<Result> BindAsync<T>(this Task<Result<T>> task, Func<T, Task<Result>> function)
    {
        var result = await task;
        if (result.IsSuccessful == false)
        {
            return Result.Failed(result.Error);
        }

        return await function(result.Value);
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
    public static async Task<Result> BindAsync<T>(this Task<Result<T>> task, Func<T, Result> function)
    {
        var result = await task;
        if (result.IsSuccessful == false)
        {
            return Result.Failed(result.Error);
        }

        return function(result.Value);
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
    public static async Task<Result> BindAsync(this Task<Result> task, Func<Task<Result>> function)
    {
        var result = await task;
        if (result.IsSuccessful == false)
        {
            return Result.Failed(result.Error);
        }

        return await function();
    }

    /// <summary>
    /// Asynchronously chains the current <see cref="Result"/> with another operation that
    /// itself produces a <see cref="Result{U}"/>.
    /// </summary>
    /// <typeparam name="U">The type of the value contained in the result returned by <paramref name="function"/>.</typeparam>
    /// <param name="function">
    /// A function to execute if this result is successful. It returns a
    /// new <see cref="Result{U}"/>
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
    public static async Task<Result<U>> BindAsync<U>(this Task<Result> task, Func<Task<Result<U>>> function)
    {
        var result = await task;
        if (result.IsSuccessful == false)
        {
            return Result.Failed<U>(result.Error);
        }

        return await function();
    }

    /// <summary>
    /// Asynchronously chains the current <see cref="Result"/> with another operation that
    /// itself produces a <see cref="Result{U}"/>.
    /// </summary>
    /// <typeparam name="U">The type of the value contained in the result returned by <paramref name="function"/>.</typeparam>
    /// <param name="function">
    /// A function to execute if this result is successful. It returns a
    /// new <see cref="Result{U}"/>
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
    public static async Task<Result<U>> BindAsync<U>(this Task<Result> task, Func<Result<U>> function)
    {
        var result = await task;
        if (result.IsSuccessful == false)
        {
            return Result.Failed<U>(result.Error);
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
    public static async Task<Result<U>> MapAsync<T, U>(this Task<Result<T>> task, Func<T, Task<U>> mapper)
    {
        var result = await task;
        if (result.IsSuccessful == false)
        {
            return Result.Failed<U>(result.Error);
        }

        var newValue = await mapper(result.Value);
        return Result.From(newValue);
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
    public static async Task<Result<U>> MapAsync<T, U>(this Task<Result<T>> task, Func<T, U> mapper)
    {
        var result = await task;
        if (result.IsSuccessful == false)
        {
            return Result.Failed<U>(result.Error);
        }

        var newValue = mapper(result.Value);
        return Result.From(newValue);
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
    public static async Task<Result<T>> TapAsync<T>(this Task<Result<T>> task, Func<T, Task> action)
    {
        var result = await task;
        if (result.IsSuccessful)
        {
            await action(result.Value);
        }

        return result;
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
    public static async Task<Result> TapAsync(this Task<Result> task, Func<Task> action)
    {
        var result = await task;
        if (result.IsSuccessful)
        {
            await action();
        }

        return result;
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
    public static async Task<U> MatchAsync<T, U>(this Task<Result<T>> task, Func<T, Task<U>> onSuccess, Func<Error, Task<U>> onFailure)
    {
        var result = await task;
        return result.IsSuccessful ? await onSuccess(result.Value) : await onFailure(result.Error);
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
    public static async Task<U> MatchAsync<T, U>(this Task<Result<T>> task, Func<T, U> onSuccess, Func<Error, U> onFailure)
    {
        var result = await task;
        return result.IsSuccessful ? onSuccess(result.Value) : onFailure(result.Error);
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
    public static async Task<U> MatchAsync<U>(this Task<Result> task, Func<Task<U>> onSuccess, Func<Error, Task<U>> onFailure)
    {
        var result = await task;
        return result.IsSuccessful ? await onSuccess() : await onFailure(result.Error);
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
    public static async Task<Result<T>> EnsureAsync<T>(this Task<Result<T>> task, Func<Task<Result>> process)
    {
        var outerResult = await task;
        if (outerResult.IsSuccessful)
        {
            var innerResult = await process();
            if (innerResult.IsSuccessful == false)
            {
                return Result.Failed<T>(innerResult.Error);
            }
        }

        return outerResult;
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
    public static async Task<Result<T>> EnsureAsync<T>(this Task<Result<T>> task, Func<T, Task<Result>> process)
    {
        var outerResult = await task;
        if (outerResult.IsSuccessful)
        {
            var innerResult = await process(outerResult.Value);
            if (innerResult.IsSuccessful == false)
            {
                return Result.Failed<T>(innerResult.Error);
            }
        }

        return outerResult;
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
    public static async Task<Result> EnsureAsync(this Task<Result> task, Func<Task<Result>> function)
    {
        var outerResult = await task;
        if (outerResult.IsSuccessful)
        {
            var innerResult = await function();
            if (innerResult.IsSuccessful == false)
            {
                return Result.Failed(innerResult.Error);
            }
        }

        return outerResult;
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
    public static async Task<Result<(T, U)>> CombineAsync<T, U>(this Task<Result<T>> task, Task<Result<U>> otherResult)
    {
        var resultA = await task;
        if (resultA.IsSuccessful == false)
        {
            return Result.Failed<(T, U)>(resultA.Error);
        }

        var resultB = await otherResult;
        if (resultB.IsSuccessful == false)
        {
            return Result.Failed<(T, U)>(resultB.Error);
        }

        return Result.From((resultA.Value, resultB.Value));
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
    public static async Task<Result<(T, U)>> CombineAsync<T, U>(this Task<Result<T>> task, Result<U> otherResult)
    {
        var resultA = await task;
        if (resultA.IsSuccessful == false)
        {
            return Result.Failed<(T, U)>(resultA.Error);
        }

        if (otherResult.IsSuccessful == false)
        {
            return Result.Failed<(T, U)>(otherResult.Error);
        }

        return Result.From((resultA.Value, otherResult.Value));
    }

    /// <summary>
    /// Asynchronously applies a mutation to the value of this <see cref="Result{T}"/> if it is successful.
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
    public static async Task<Result<T>> ApplyAsync<T>(this Task<Result<T>> task, Action<T> mutator)
    {
        var result = await task;

        if (result.IsSuccessful)
        {
            mutator(result.Value);
        }

        return result;
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
    public static async Task<Result<T>> ApplyAsync<T>(this Task<Result<T>> task, Func<T, Task> asyncMutator)
    {
        var result = await task;

        if (result.IsSuccessful)
        {
            await asyncMutator(result.Value);
        }

        return result;
    }
}
