# Sprout.Exam.WebApp README

### If we are going to deploy this on production, what do you think is the next improvement that you will prioritize next?

### This can be a feature, a tech debt, or an architectural design.

One thing I noticed is that the employee index page is not paginated.
I also noticed that there are no timestamps for createdAt and modifiedAt in the tables. In my experience, having timestamps have proven to be a good thing specially when tracing issues.
Speaking of tracing, I would also add a logger in the classes which can again help with tracing issues.
