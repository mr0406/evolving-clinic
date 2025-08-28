# 🏥 Evolving Clinic

A Domain-Driven Design experiment showing how business requirements drive domain model evolution.

## 💡 Motivation

Most DDD examples you see online are completely planned upfront with all patterns decided from day one, or they're overly simplified without real business complexity. But that's not how real software development works. Requirements evolve. Business changes. New challenges emerge that you couldn't predict.

Rather than complaining about it, I decided to try creating something better myself.

This project shows **Domain-Driven Design in action** - starting with a simple software system for a clinic and watching as new business requirements force domain model evolution, refactoring, and the gradual emergence of DDD patterns.

## 🎯 The Approach

**No event storming.** **No big upfront design.** **No predictions about what patterns we'll need.**

Instead, we adapt to unexpected requirements as they come:

- Sometimes I'll think of them
- Sometimes contributors will suggest ideas (feel free to open issues with business scenarios!)
- Sometimes AI will generate interesting challenges

This unpredictability is the whole point.

## 🔄 My Take on Domain-Driven Design

In my opinion, all those fancy DDD terms and patterns exist for one simple reason: to make change easy and keep the system reliable as it grows. When new requirements hit (and they always do), a well-modeled domain bends instead of breaking.

I'll start simple and do my best from the beginning, but I genuinely don't know what requirements are coming next. Let's see what patterns naturally emerge when business reality puts pressure on our domain model.

## 🎮 The Goal

**Have fun with domain modeling and testing.** I love this whole DDD thing, and this project is as much for enjoyment as it is for demonstration.

Show how sophisticated domain design isn't planned upfront - it evolves under pressure while maintaining system integrity through solid business logic and comprehensive test coverage.

## ✅ What This Project IS

- Domain layer + Application layer
- Lots of tests
- DDD patterns emerging organically
- Real evolution under business pressure

## ❌ What This Project ISN'T

- Infrastructure complexity
- Performance optimization
- Fancy frameworks
- Production-ready persistence

Everything other than domain and tests will be naive - repositories with in-memory lists, simple implementations. I've built whole production systems before, but that's not the point here.

Some might call this approach naive, but I disagree. Domain evolution is no less challenging than microservices, Kubernetes, or complex database migrations - it's just different. And honestly, there aren't many good examples of this kind of work out there.

## 🏥 Why Healthcare?

Everyone knows something about clinics - patients, appointments, basic concepts. But I've never worked in this industry, which means I won't overthink the domain.

Perfect for discovering patterns as we go, not predicting them upfront.
